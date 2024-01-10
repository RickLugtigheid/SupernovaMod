using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using SupernovaMod.Common.Systems;

namespace SupernovaMod.Content.Projectiles.Thrown
{
    public class CarnageTrowingKniveProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Carnage Trowing Knive");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ThrowingKnife);
            AIType = ProjectileID.ThrowingKnife;

            Projectile.penetrate = 2;
            IsStickingToTarget = false;
			Projectile.DamageType = GlobalModifiers.DamageClass_ThrowingRanged;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            return projHitbox.Intersects(targetHitbox);
        }

        public override void OnKill(int timeLeft)
        {
            //SoundEngine.PlaySound(0, (int)Projectile.position.X, (int)Projectile.position.Y, 1, 1f, 0f);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            // Spawn dust on kill
            //
            for (int x = 0; x <= 7; x++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bone, -Projectile.velocity.X, -Projectile.velocity.Y, 80, default, 1);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = false; //this make so the dust has no gravity
                Main.dust[dust].velocity *= Main.rand.NextFloat(.2f, .4f);
            }

            Vector2 usePos = Projectile.position;
            Vector2 rotVector = (Projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2();
            usePos += rotVector * 16f;

            int item = 0;
            // Give the projectile a 7% chance to drop it's item after death
            //
            if (Main.rand.NextFloat() < 0.07f)
            {
                item = Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.position, ModContent.ItemType<Items.Weapons.Throwing.CarnageTrowingKnive>());
            }

            if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
            {
                NetMessage.SendData(MessageID.KillProjectile);
            }
        }


        // Are we sticking to a target?
        public bool IsStickingToTarget { get; set; }

        // Index of the current target
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

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);

            // we are sticking to a target
            IsStickingToTarget = true;

            // Reset our AIType to use our AI
            //AIType = 0;

            // Set the target whoAmI
            TargetWhoAmI = target.whoAmI;

            // Change velocity based on delta center of targets (difference between entity centers)
            Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;

            // netUpdate this projectile
            Projectile.netUpdate = true;

            // Makes sure the sticking javelins do not deal damage anymore
            Projectile.damage = 0;
        }

        //public override bool PreAI() => !IsStickingToTarget; // When we are sticking to the target we will not use the normal ai

        /*public override void AI()
		{
            // Check if we should stick to the target
            //
            if (IsStickingToTarget)
			{
                StickyAI();
            }
            else
			{
                base.AI();
            }
        }*/

        private void StickyAI()
        {
            // These 2 could probably be moved to the ModifyNPCHit hook, but in vanilla they are present in the AI
            Projectile.ignoreWater = true; // Make sure the projectile ignores water
            Projectile.tileCollide = false; // Make sure the projectile doesn't collide with tiles anymore
            const int aiFactor = 15; // Change this factor to change the 'lifetime' of this sticking javelin
            Projectile.localAI[0] += 1f;

            // Every 30 ticks, the javelin will perform a hit effect
            bool hitEffect = Projectile.localAI[0] % 30f == 0f;
            int projTargetIndex = TargetWhoAmI;
            if (Projectile.localAI[0] >= 60 * aiFactor || projTargetIndex < 0 || projTargetIndex >= 200)
            { // If the index is past its limits, kill it
                Projectile.Kill();
            }
            else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
            { // If the target is active and can take damage
              // Set the projectile's position relative to the target's center
                Projectile.Center = Main.npc[projTargetIndex].Center - Projectile.velocity * 2f;
                Projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
                if (hitEffect)
                { // Perform a hit effect here
                    Main.npc[projTargetIndex].HitEffect(0, 1.0);
                }
            }
            else
            { // Otherwise, kill the projectile
                Projectile.Kill();
            }
        }
    }
}
