using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Audio;

namespace SupernovaMod.Content.Projectiles.Ranged
{
    public class VerglasIcicle : ModProjectile
    {
        /// <summary>
        /// The amount of ticks before our sticking icicle should do damage.
        /// </summary>
        private const float TICKS_BEFORE_TARGET_DAMAGE = 30;
        private const int MAX_STICKY_PROJECTILES = 6;
        private readonly int _stickyDebuffId = ModContent.BuffType<Buffs.Debuffs.VerglasIcicleDebuff>();
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Icicle");
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Blizzard);
            //Projectile.width = 14;
            //Projectile.height = 34;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 800;  //The amount of time the projectile is alive for
            AIType = ProjectileID.Blizzard;
        }
        public override void AI()
        {
            if (IsStickingToTarget)
            {
                // When sticking to a target call a different ai
                StickyAI();
                return;
            }

            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.width, DustID.IceRod, Projectile.velocity.X * .5f, Projectile.velocity.Y * .5f);
                Main.dust[dust].noGravity = true;
            }

            // Call the blizzard projectile AI
            base.AI();
        }

        private void StickyAI()
        {
            // These 2 could probably be moved to the ModifyNPCHit hook, but in vanilla they are present in the AI
            Projectile.ignoreWater = true; // Make sure the projectile ignores water
            Projectile.tileCollide = false; // Make sure the projectile doesn't collide with tiles anymore
            const int aiFactor = 15; // Change this factor to change the 'lifetime' of this sticking javelin
            Projectile.localAI[0] += 1f;

            // Every TICKS_BEFORE_TARGET_DAMAGE ticks deal damage to the target
            bool shouldHitTarget = Projectile.localAI[0] % TICKS_BEFORE_TARGET_DAMAGE == 0f;
            int projTargetIndex = TargetWhoAmI;

            // If the index is past its limits, kill this projectile
            //
            if (Projectile.localAI[0] >= 60 * aiFactor || projTargetIndex < 0 || projTargetIndex >= 200)
            {
                Projectile.Kill();
            }
            // Check if the target is active and can take damage
            //
            else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
            {

                // Set the projectile's position relative to the target's center
                Projectile.Center = Main.npc[projTargetIndex].Center - Projectile.velocity * 2f;
                Projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;

                // Make sure our buff is applied
                if (!Main.npc[projTargetIndex].HasBuff(_stickyDebuffId))
                {
                    Main.npc[projTargetIndex].AddBuff(_stickyDebuffId, (int)TICKS_BEFORE_TARGET_DAMAGE);
                }

                // Check if we should hit the target
                //
                if (shouldHitTarget)
                {
                    Main.npc[projTargetIndex].HitEffect(0);
                }
            }
            // Otherwise, kill the projectile
            else
            {
                Projectile.Kill();
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(2))
            {
                target.AddBuff(BuffID.Frostburn, 80);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            return projHitbox.Intersects(targetHitbox);
        }

        /// <summary>
        /// If the projectile is sticking to a target
        /// </summary>
        public bool IsStickingToTarget { get; set; }

        /// <summary>
        /// The target currently stuck to
        /// </summary>
        public int TargetWhoAmI
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            // If attached to an NPC, draw behind tiles (and the npc) if that NPC is behind tiles, otherwise just behind the NPC.
            if (IsStickingToTarget) // or if(isStickingToTarget) since we made that helper method.
            {
                int npcIndex = (int)Projectile.ai[1];
                if (npcIndex >= 0 && npcIndex < 200 && Main.npc[npcIndex].active)
                {
                    if (Main.npc[npcIndex].behindTiles)
                    {
                        behindNPCsAndTiles.Add(index);
                    }
                    else
                    {
                        behindNPCs.Add(index);
                    }

                    return;
                }
            }
            // Since we aren't attached, add to this list
            behindProjectiles.Add(index);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);

            // we are sticking to a target
            IsStickingToTarget = true;

            // Reset our AIType to use our AI
            Projectile.aiStyle = -1;
            AIType = 0;

            // Set the target whoAmI
            TargetWhoAmI = target.whoAmI;

            // Change velocity based on delta center of targets (difference between entity centers)
            Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;

            // netUpdate this projectile
            Projectile.netUpdate = true;

            // Makes sure the sticking projectiles do not deal damage anymore
            Projectile.damage = 0;

            // Update our sticking projectiles
            UpdateStickyProjectiles(target);
        }

        private readonly Point[] _stickingJavelins = new Point[MAX_STICKY_PROJECTILES];
        private void UpdateStickyProjectiles(NPC target)
        {
            int currentJavelinIndex = 0; // The javelin index

            for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
            {
                Projectile currentProjectile = Main.projectile[i];
                if (i != Projectile.whoAmI // Make sure the looped projectile is not the current javelin
                    && currentProjectile.active // Make sure the projectile is active
                    && currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
                    && currentProjectile.type == Projectile.type // Make sure the projectile is of the same type as this javelin
                    && currentProjectile.ModProjectile is VerglasIcicle javelinProjectile // Use a pattern match cast so we can access the projectile like an ExampleJavelinProjectile
                    && javelinProjectile.IsStickingToTarget // the previous pattern match allows us to use our properties
                    && javelinProjectile.TargetWhoAmI == target.whoAmI)
                {

                    _stickingJavelins[currentJavelinIndex++] = new Point(i, currentProjectile.timeLeft); // Add the current projectile's index and timeleft to the point array
                    if (currentJavelinIndex >= _stickingJavelins.Length)  // If the javelin's index is bigger than or equal to the point array's length, break
                        break;
                }
            }

            // Remove the oldest sticky javelin if we exceeded the maximum
            if (currentJavelinIndex >= MAX_STICKY_PROJECTILES)
            {
                int oldJavelinIndex = 0;
                // Loop our point array
                for (int i = 1; i < MAX_STICKY_PROJECTILES; i++)
                {
                    // Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
                    if (_stickingJavelins[i].Y < _stickingJavelins[oldJavelinIndex].Y)
                    {
                        oldJavelinIndex = i; // Remember the index of the removed javelin
                    }
                }
                // Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
                Main.projectile[_stickingJavelins[oldJavelinIndex].X].Kill();
            }
        }


        public override void Kill(int timeLeft)
        {
            for (int x = 0; x <= 7; x++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceRod, -Projectile.velocity.X, -Projectile.velocity.Y, 80, default, 1);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = false; //this make so the dust has no gravity
                Main.dust[dust].velocity *= Main.rand.NextFloat(.2f, .4f);
            }

            SoundEngine.PlaySound(GetRandomIceStruckSound(), Projectile.position);
        }

        private SoundStyle GetRandomIceStruckSound()
        {
            switch (Main.rand.Next(0, 2))
            {
                default:
                    return SoundID.Item48;
                case 1:
                    return SoundID.Item49;
                case 2:
                    return SoundID.Item50;
            }
        }
    }
}
