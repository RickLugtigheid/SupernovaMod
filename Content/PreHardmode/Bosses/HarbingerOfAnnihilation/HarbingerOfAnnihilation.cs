using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Supernova.Common.Systems;
using Terraria.GameContent.ItemDropRules;
using Supernova.Api.Core;
using Terraria.GameContent.Bestiary;
using Supernova.Common.ItemDropRules.DropConditions;

namespace Supernova.Content.PreHardmode.Bosses.HarbingerOfAnnihilation
{
	[AutoloadBossHead]
	public class HarbingerOfAnnihilation : ModBossNpc
    {
        /* Stats */
        private int _stage = 0;
        public int smallAttackDamage = 17;
        public int largeAttackDamage = 26;
        const float ShootKnockback = 5f;
        const int ShootDirection = 9;

        bool spin = false;
        bool _despawn = false;

        /* Stage Attacks */
        public string[] stage0 = new string[] { "atkShootTeleportLeft", "atkEnergyBall", "atkShootTeleportRight", "atkEnergyBall", "atkEnergyBall" };
        public string[] stage1 = new string[] { "atkEnergyBall", "atkEnergyBall", "atkShootTeleportLeft", "atkEnergyBall", "atkEnergyBall", "atkEnergyBall", "atkShootTeleportRight" };
        public string[] stage2 = new string[] { "atkRage", "atkRage", "atkShootTeleportRage" };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harbinger of Annihilation");

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("TODO."),
            });
        }

        public override void SetDefaults()
        {
            targetOffset = new Vector2(-170, -270);
            velMax = 5;
            velAccel = .35f;

            attackPointer = 0;
            attacks = stage0;

            NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 
            NPC.lifeMax = 1950; // The Max HP the boss has on Normal
            NPC.damage = 32; // The base damage value the boss has on Normal
            NPC.defense = 10; // The base defense on Normal
            NPC.knockBackResist = 0f; // No knockback
            NPC.width = 214;
            NPC.height = 214;
            NPC.value = 10000;
            NPC.npcSlots = 1f; // The higher the number, the more NPC slots this NPC takes.
            NPC.boss = true; // Is a boss
            NPC.lavaImmune = true; // Not hurt by lava
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = true; // Will not collide with the tiles. 
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicID.Boss1;
            // bossBag = mod.ItemType("SepticFleshBossBag"); // Needed for the NPC to drop loot bag.
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            /*if (Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HarbingersCrest>()));
            }*/
            npcLoot.Add(ItemDropRule.ByCondition(new ExpertModDropCondition(), ModContent.ItemType<HarbingersCrest>()));

            for (int i = 0; i < Main.rand.Next(1, 2); i++)
            {
                switch (Main.rand.Next(2))
				{
                    case 0:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HarbingersSlicer>()));
                        break;
                    case 1:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HarbingersKnell>()));
                        break;
                    case 2:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HarbingersPick>()));
                        break;
                }
            }

            // For settings if the boss has been downed
            SupernovaBosses.downedHarbingerOfAnnihilation = true;

            base.ModifyNPCLoot(npcLoot);
        }

        bool init = false;
        public override void AI()
		{

            if (init == false)
            {
                attacks = stage0;
                NPC.netUpdate = true;
                init = true;
            }
            if (spin == true)
                NPC.rotation += (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + MathHelper.ToRadians(30);

            // Attack
            if (!_despawn) Attack();

            // Handle despawning
            DespawnHandler();

            // Move
            target = targetPlayer.Center;
            target.X += targetOffset.X;
            target.Y += targetOffset.Y;

            if (!_despawn)
            {
                AnimateRotation();
                Move();
            }
        }
        #region Attacks
        public void atkShootTeleportLeft()
		{
            NPC.ai[0]++;
            if (NPC.ai[0] == 10)
            {
                velMax = 23;
                velMagnitude = 15;
                velAccel = 1f;
                NPC.position.X = (Main.player[NPC.target].position.X + -200);
                NPC.position.Y = (Main.player[NPC.target].position.Y + -250);
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UndergroundHallowedEnemies);
                    Main.dust[dust].scale = 2.5f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0;
                }
            }
            if (NPC.ai[0] == 100)
            {
                targetRotation = MathHelper.ToRadians(-47.5f);
                targetOffset = new Vector2(-250, -300);
                ShootPlus();
            }
            else if (NPC.ai[0] == 150)
            {
                targetRotation = MathHelper.ToRadians(-87.5f);
                ShootX();
            }
            else if (NPC.ai[0] == 250)
            {
                targetRotation = MathHelper.ToRadians(-47.5f);
                targetOffset.X = -targetOffset.X;
                ShootPlus();
            }
            else if (NPC.ai[0] == 300)
            {
                targetRotation = MathHelper.ToRadians(0);
                ShootX();
            }
            else if (NPC.ai[0] == 340)
            {
                velAccel = .03f;
                velMax = 4;
                NPC.defense = 7;
                NPC.position.X = (Main.player[NPC.target].position.X + 500);
                NPC.position.Y = (Main.player[NPC.target].position.Y);
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 71);
                    Main.dust[dust].scale = 2.5f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0;
                }
                targetOffset = Vector2.Zero;
            }
            else if (NPC.ai[0] == 480)
            {
                velAccel = .03f;
                velMax = 2;
                targetOffset.Y = -450;
            }
            if (NPC.ai[0] >= 520)
            {
                velAccel = .3f;
                velMax = 4;
                NPC.defense = 10;

                NPC.ai[0] = 0;
                attackPointer++;
            }
        }
        public void atkShootTeleportRight()
		{
            NPC.ai[0]++;
            if (NPC.ai[0] == 10)
            {
                velMax = 23;
                velMagnitude = 15;
                velAccel = 1f;
                NPC.position.X = (Main.player[NPC.target].position.X + 300);
                NPC.position.Y = (Main.player[NPC.target].position.Y + -250);
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UndergroundHallowedEnemies);
                    Main.dust[dust].scale = 2.5f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0;
                }
            }
            if (NPC.ai[0] == 100)
            {
                targetRotation = MathHelper.ToRadians(47.5f);
                targetOffset = new Vector2(250, -300);
                ShootPlus();
            }
            else if (NPC.ai[0] == 150)
            {
                targetRotation = MathHelper.ToRadians(90);
                ShootX();
            }
            else if (NPC.ai[0] == 250)
            {
                targetRotation = MathHelper.ToRadians(47.5f);
                targetOffset.X = -targetOffset.X;
                ShootPlus();
            }
            else if (NPC.ai[0] == 300)
            {
                targetRotation = MathHelper.ToRadians(0);
                ShootX();
            }
            else if (NPC.ai[0] == 340)
            {
                velAccel = .03f;
                velMax = 2;
                NPC.defense = 7;
                NPC.position.X = (Main.player[NPC.target].position.X - 500);
                NPC.position.Y = (Main.player[NPC.target].position.Y);
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 71);
                    Main.dust[dust].scale = 2.5f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0;
                }
                targetOffset = Vector2.Zero;
            }
            else if (NPC.ai[0] == 480)
            {
                velAccel = .03f;
                velMax = 2;
                targetOffset.Y = -450;
            }
            if (NPC.ai[0] >= 520)
            {
                velAccel = .3f;
                velMax = 4;
                NPC.defense = 10;

                NPC.ai[0] = 0;
                attackPointer++;
            }
        }

        public void atkEnergyBall()
		{
            NPC.ai[0]++;
            targetOffset = new Vector2(0, -200);
            velMax = 3;
            if (NPC.ai[0] >= 30)
			{
                // Shoot
                float Speed = 14;
                int type = ProjectileID.DD2DarkMageBolt;
                SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                Vector2 vector8 = new Vector2(NPC.Center.X, NPC.Center.Y);

                float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.5f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.5f)));

                Projectile.NewProjectile(NPC.GetSource_FromAI(), vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * Speed)), (float)(-(Math.Sin(rotation) * Speed)), type, largeAttackDamage, 0f, 0);

                // Reset
                NPC.ai[0] = 0;
                attackPointer++;
            }
        }
        public void atkShootTeleportRage()
        {
            NPC.ai[0]++;
            if (NPC.ai[0] == 10)
            {
                velMax = 18;
                NPC.position.X = (Main.player[NPC.target].position.X + -200);
                NPC.position.Y = (Main.player[NPC.target].position.Y + -250);
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 71);
                    Main.dust[dust].scale = 2.5f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].velocity *= 0f;
                }
            }
            if (NPC.ai[0] == 100)
            {
                velAccel = .5f;
                NPC.frameCounter += 2;
                targetOffset = new Vector2(-250, -300);
                ShootPlus();
            }
            else if (NPC.ai[0] == 125)
                ShootPlus();
            else if (NPC.ai[0] == 150)
            {
                NPC.frameCounter += 2;

                ShootX();
                NPC.frameCounter = 0;
            }
            else if (NPC.ai[0] == 175)
                ShootX();
            else if (NPC.ai[0] == 250)
            {
                NPC.frameCounter += 2;

                targetOffset.X = -targetOffset.X;
                ShootPlus();
            }
            else if (NPC.ai[0] == 275)
                ShootPlus();
            else if (NPC.ai[0] == 300)
            {
                NPC.frameCounter += 2;

                ShootX();
                NPC.frameCounter = 0;
            }
            else if (NPC.ai[0] == 325)
                ShootX();
            else if (NPC.ai[0] == 390)
            {
                velAccel = .03f;
                velMax = 2;
                NPC.defense = 7;
                NPC.position.X = (Main.player[NPC.target].position.X + 500);
                NPC.position.Y = (Main.player[NPC.target].position.Y + 150);
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 71);
                    Main.dust[dust].scale = 2.5f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].velocity *= 0f;
                }

                targetOffset = Vector2.Zero;
            }
            else if (NPC.ai[0] == 470)
            {
                velAccel = .03f;
                velMax = 2;
                targetOffset.Y = -450;
            }
            if (NPC.ai[0] >= 510)
            {
                velMax = 5;
                velAccel = .3f;
                NPC.defense = 5;

                NPC.ai[0] = 0;
                attackPointer++;
            }
        }
        public void atkRage()
		{
            velMax = 3;
            NPC.ai[0]++;
            NPC.defense = 16;

            targetOffset = Vector2.Zero;

            spin = true;
            if (NPC.ai[0] == 10)
                velAccel = .05f;
            else if (NPC.ai[0] == 50)
                Shoot();
            else if (NPC.ai[0] == 60)
                Shoot();
            else if (NPC.ai[0] == 70)
                Shoot();
            else if (NPC.ai[0] == 75)
                Shoot();
            else if (NPC.ai[0] >= 80)
            {
                Shoot();
                // Reset
                NPC.ai[0] = 0;
                attackPointer++;
            }
        }
        #endregion
        private void Shoot()
        {
            float Speed = Main.rand.Next(5, 8);  //projectile speed

            int type = ModContent.ProjectileType<HarbingerMissile>();  //put your projectile
            SoundEngine.PlaySound(/*23*/SoundID.Item23, NPC.position);
            Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));

            float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.5f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.5f)));

            Projectile.NewProjectile(NPC.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -2), (float)((Math.Sin(rotation) * Speed) * -1), type, smallAttackDamage, 0f, 0);

            NPC.ai[1] = 0;
        }
        void ShootPlus()
        {
            int Type = ModContent.ProjectileType<HarbingerMissile>();
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, -ShootDirection, -ShootDirection, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, ShootDirection, -ShootDirection, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, -ShootDirection, ShootDirection, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, ShootDirection, ShootDirection, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
        }
        void ShootX()
        {
            int Type = ModContent.ProjectileType<HarbingerMissile>();
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, -ShootDirection, 0, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, ShootDirection, 0, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, ShootDirection, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, -ShootDirection, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
        }

        public Vector2 target;
        public Vector2 targetOffset = new Vector2(0, -200);
        public int velMax = 0;
        public float velAccel = 0;
        public float targetVel = 0;
        public float velMagnitude = 0;
        private void Move()
        {
            //float dist = (float)(Math.Sqrt(((target.X - npc.Center.X) * (target.X - npc.Center.X)) + ((target.Y - npc.Center.Y) * (target.Y - npc.Center.Y))));
            float dist = Vector2.Distance(NPC.Center, target);
            targetVel = dist / 20;
            if (targetVel >= 10) targetVel = 10;
            // Accel if our velocity is smaller than the taget velocity and max velocity
            if (velMagnitude < velMax && velMagnitude < targetVel)
                velMagnitude += velAccel;

            if (velMagnitude > targetVel)
                velMagnitude -= velAccel;

            // Clamp the velocity
            if (velMagnitude > velMax)
                velMagnitude = velMax;

            // Make sure we don't devide by 0
            if (dist != 0)
                // Move to 'target'
                NPC.velocity = NPC.DirectionTo(target) * velMagnitude;
        }
        private void DespawnHandler()
        {
            if (!targetPlayer.active || targetPlayer.dead)
            {
                NPC.TargetClosest(false);
                targetPlayer = Main.player[NPC.target];
                if (!targetPlayer.active || targetPlayer.dead)
				{
                    _despawn = true;
					NPC.velocity = new Vector2(0f, -10f);
                    if (NPC.timeLeft > 5)
                    {
                        NPC.timeLeft = 5;
                    }
                    return;
                }
            }
        }
        float targetRotation = 0;
        float rotateSpeed = MathHelper.ToRadians(5);
        public void AnimateRotation()
		{
            if (NPC.rotation > targetRotation
                && (NPC.rotation + rotateSpeed) > targetRotation)
                NPC.rotation -= rotateSpeed;
         
            else if (NPC.rotation < targetRotation
                && (NPC.rotation + rotateSpeed) < targetRotation)
                NPC.rotation += rotateSpeed;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Main glowmask
            /*Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 drawPos = NPC.Center - Main.screenPosition;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);

            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, drawPos, NPC.frame, new Color(180, 180, 180, 245), NPC.rotation, drawOrigin, 1f, effects, 0f);*/
            try
			{
                // Stages
                if (NPC.life <= (NPC.lifeMax * .35f))
                {
                    attacks = stage2;
                    _stage = 2;
                }
                else if (NPC.life <= (NPC.lifeMax * .7f))
                {
                    attacks = stage1;
                    _stage = 1;
                }

                // Reset attack pointer when we have done all the attacks for this stage
                if (attackPointer >= attacks.Length) attackPointer = 0;
            }
			catch
			{

			}
            return true;
        }
    }
}
