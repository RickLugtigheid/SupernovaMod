using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Supernova.Npcs.Bosses.HarbingerOfAnnihilation
{
	[AutoloadBossHead]
	public class HarbingerOfAnnihilation : Boss
	{
        /* Stats */
        public int smallAttackDamage = 15;
        public int largeAttackDamage = 27;
        const float ShootKnockback = 5f;
        const int ShootDirection = 7;

        bool spin = false;
        bool _despawn = false;

        /* Stage Attacks */
        public string[] stage0 = new string[] { "atkShootTeleport", "atkEnergyBall", "atkShootTeleport", "atkEnergyBall", "atkEnergyBall" };
        public string[] stage1 = new string[] { "atkEnergyBall", "atkEnergyBall", "atkShootTeleport", "atkEnergyBall", "atkEnergyBall", "atkEnergyBall", "atkShootTeleport" };
        public string[] stage2 = new string[] { "atkRage", "atkRage", "atkShootTeleportRage" };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harbinger of Annihilation");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            targetOffset = new Vector2(-170, -270);
            velMax = 5;
            velAccel = .3f;

            attackPointer = 0;
            attacks = stage0;

            npc.aiStyle = -1; // Will not have any AI from any existing AI styles. 
            npc.lifeMax = 1950; // The Max HP the boss has on Normal
            npc.damage = 32; // The base damage value the boss has on Normal
            npc.defense = 10; // The base defense on Normal
            npc.knockBackResist = 0f; // No knockback
            npc.width = 100;
            npc.height = 100;
            npc.value = 10000;
            npc.npcSlots = 1f; // The higher the number, the more NPC slots this NPC takes.
            npc.boss = true; // Is a boss
            npc.lavaImmune = true; // Not hurt by lava
            npc.noGravity = true; // Not affected by gravity
            npc.noTileCollide = true; // Will not collide with the tiles. 
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            music = MusicID.Boss1;
            // bossBag = mod.ItemType("SepticFleshBossBag"); // Needed for the NPC to drop loot bag.
        }
        public override void NPCLoot()
        {
            // Drop the correct drops
            if (Main.expertMode)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HarbingersCrest"));
            for(int i = 0; i < Main.rand.Next(1, 2); i++)
                switch(Main.rand.Next(3))
			    {
                    case 0:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HarbingersSlicer"));
                        break;
                    case 1:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HarbingersKnell"));
                        break;
                    default:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HarbingersPick"));
                        break;
                }

            // For settings if the boss has been downed
            SupernovaWorld.downedHarbingerOfAnnihilation = true;
        }

        bool init = false;
        public override void AI()
		{

            if (init == false)
            {
                attacks = stage0;
                npc.netUpdate = true;
                init = true;
            }
            if (spin == true)
			{
                npc.rotation += (float)System.Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X) + MathHelper.ToRadians(30);
            }

            // Attack
            if (!_despawn) Attack();

            // Handle despawning
            DespawnHandler();

            // Move
            target = targetPlayer.Center;
            target.X += targetOffset.X;
            target.Y += targetOffset.Y;

            if (!_despawn) Move();
        }
        #region Attacks
        public void atkShootTeleport()
		{
            npc.ai[0]++;
            if (npc.ai[0] == 10)
            {
                velMax = 10;
                npc.position.X = (Main.player[npc.target].position.X + -200);
                npc.position.Y = (Main.player[npc.target].position.Y + -250);
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 71);
                    Main.dust[dust].scale = 2.5f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].velocity *= 0f;
                }
            }
            if (npc.ai[0] == 100)
            {
                npc.frameCounter += 2;
                targetOffset = new Vector2(-250, -300);
                ShootPlus();
            }
            else if (npc.ai[0] == 150)
            {
                npc.frameCounter += 2;

                ShootX();
                npc.frameCounter = 0;
            }
            else if (npc.ai[0] == 250)
            {
                npc.frameCounter += 2;
                targetOffset.X = -targetOffset.X;
                ShootPlus();
            }
            else if (npc.ai[0] == 300)
            {
                npc.frameCounter += 2;

                ShootX();
                npc.frameCounter = 0;
            }
            else if (npc.ai[0] == 340)
            {
                velAccel = .03f;
                velMax = 3;
                npc.defense = 7;
                npc.position.X = (Main.player[npc.target].position.X + 300);
                npc.position.Y = (Main.player[npc.target].position.Y);
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 71);
                    Main.dust[dust].scale = 2.5f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].velocity *= 0f;
                }
                targetOffset = Vector2.Zero;
            }
            else if (npc.ai[0] == 480)
            {
                targetOffset.Y = -200;
            }
            if (npc.ai[0] >= 500)
            {
                velAccel = .2f;
                velMax = 5;
                npc.defense = 10;

                npc.ai[0] = 0;
                attackPointer++;
            }
        }

        public void atkEnergyBall()
		{
            npc.ai[0]++;
            targetOffset = new Vector2(0, -200);
            if (npc.ai[0] >= 30)
			{
                // Shoot
                float Speed = 14;
                int type = ProjectileID.DD2DarkMageBolt;
                Main.PlaySound(SoundID.Item20, npc.Center);
                Vector2 vector8 = new Vector2(npc.Center.X, npc.Center.Y);

                float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.5f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.5f)));

                Projectile.NewProjectile(vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * Speed)), (float)(-(Math.Sin(rotation) * Speed)), type, largeAttackDamage, 0f, 0);

                // Reset
                npc.ai[0] = 0;
                attackPointer++;
            }
        }
        public void atkShootTeleportRage()
        {
            npc.ai[0]++;
            if (npc.ai[0] == 10)
            {
                velMax = 15;
                npc.position.X = (Main.player[npc.target].position.X + -200);
                npc.position.Y = (Main.player[npc.target].position.Y + -250);
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 71);
                    Main.dust[dust].scale = 2.5f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].velocity *= 0f;
                }
            }
            if (npc.ai[0] == 100)
            {
                velAccel = .5f;
                npc.frameCounter += 2;
                targetOffset = new Vector2(-250, -300);
                ShootPlus();
            }
            else if (npc.ai[0] == 125)
                ShootPlus();
            else if (npc.ai[0] == 150)
            {
                npc.frameCounter += 2;

                ShootX();
                npc.frameCounter = 0;
            }
            else if (npc.ai[0] == 175)
                ShootX();
            else if (npc.ai[0] == 250)
            {
                npc.frameCounter += 2;

                targetOffset.X = -targetOffset.X;
                ShootPlus();
            }
            else if (npc.ai[0] == 275)
                ShootPlus();
            else if (npc.ai[0] == 300)
            {
                npc.frameCounter += 2;

                ShootX();
                npc.frameCounter = 0;
            }
            else if (npc.ai[0] == 325)
                ShootX();
            else if (npc.ai[0] == 390)
            {
                npc.defense = 7;
                npc.position.X = (Main.player[npc.target].position.X + 300);
                npc.position.Y = (Main.player[npc.target].position.Y);
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 71);
                    Main.dust[dust].scale = 2.5f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].velocity *= 0f;
                }

                velAccel = .05f;
                targetOffset = Vector2.Zero;
            }
            else if (npc.ai[0] == 470)
            {
                targetOffset.Y = -200;
            }
            if (npc.ai[0] >= 500)
            {
                velMax = 5;
                velAccel = .3f;
                npc.defense = 5;

                npc.ai[0] = 0;
                attackPointer++;
            }
        }
        public void atkRage()
		{
            npc.ai[0]++;
            npc.defense = 16;

            targetOffset = Vector2.Zero;

            spin = true;
            if (npc.ai[0] == 10)
                velAccel = .05f;
            else if (npc.ai[0] == 50)
                Shoot();
            else if (npc.ai[0] == 60)
                Shoot();
            else if (npc.ai[0] == 70)
                Shoot();
            else if (npc.ai[0] == 75)
                Shoot();
            else if (npc.ai[0] == 80)
            {
                Shoot();
                // Reset
                npc.ai[0] = 0;
                attackPointer++;
            }
        }
        #endregion
        private void Shoot()
        {
            float Speed = Main.rand.Next(5, 8);  //projectile speed

            int type = mod.ProjectileType("HarbingerMissile");  //put your projectile
            Main.PlaySound(23, (int)npc.position.X, (int)npc.position.Y, -10);
            Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));

            float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.5f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.5f)));

            Projectile.NewProjectile(vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -2), (float)((Math.Sin(rotation) * Speed) * -1), type, smallAttackDamage, 0f, 0);

            npc.ai[1] = 0;
        }
        void ShootPlus()
        {
            int Type = mod.ProjectileType("HarbingerMissile");
            Projectile.NewProjectile(npc.position.X + 20, npc.position.Y + 20, -ShootDirection, -ShootDirection, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 20, npc.position.Y + 20, ShootDirection, -ShootDirection, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 20, npc.position.Y + 20, -ShootDirection, ShootDirection, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 20, npc.position.Y + 20, ShootDirection, ShootDirection, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
        }
        void ShootX()
        {
            int Type = mod.ProjectileType("HarbingerMissile");
            Projectile.NewProjectile(npc.position.X + 20, npc.position.Y + 20, -ShootDirection, 0, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 20, npc.position.Y + 20, ShootDirection, 0, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 20, npc.position.Y + 20, 0, ShootDirection, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 20, npc.position.Y + 20, 0, -ShootDirection, Type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
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
            float dist = Vector2.Distance(npc.Center, target);
            targetVel = dist / 20;

            // Accel if our velocity is smaller than the taget velocity and max velocity
            if (velMagnitude < velMax && velMagnitude < targetVel)
                velMagnitude += velAccel;

            if (velMagnitude > targetVel)
                velMagnitude -= velAccel;

            // Make sure we don't devide by 0
            if (dist != 0)
                // Move to 'target'
                npc.velocity = npc.DirectionTo(target) * velMagnitude;
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter %= 20;
            int frame = (int)(npc.frameCounter / 2.0);
            if (frame >= Main.npcFrameCount[npc.type]) frame = 0;
            npc.frame.Y = frame * frameHeight;
        }
        private void DespawnHandler()
        {
            if (!targetPlayer.active || targetPlayer.dead)
            {
                npc.TargetClosest(false);
                targetPlayer = Main.player[npc.target];
                if (!targetPlayer.active || targetPlayer.dead)
				{
                    _despawn = true;
					npc.velocity = new Vector2(0f, -10f);
                    if (npc.timeLeft > 5)
                    {
                        npc.timeLeft = 5;
                    }
                    return;
                }
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            try
			{
                // Stages
                if (npc.life <= (npc.lifeMax * .35f))
                    attacks = stage2;
                else if (npc.life <= (npc.lifeMax * .7f))
                    attacks = stage1;

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
