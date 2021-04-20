using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Supernova.Npcs.Bosses.FlyingTerror
{
	[AutoloadBossHead]
	public class FlyingTerror : Boss
    {
        public bool _move = true;

        /* Stats */
        public int smallAttackDamage = 22;
        public int largeAttackDamage = 37;
        const float ShootKnockback = 5f;
        const int ShootDirection = 7;

        /* Stage Attacks */
        public string[] stage0 = new string[] { "atkTeleport", "atkBomb" };
        public string[] stage1 = new string[] { "atkTeleport", "atkBomb", "atkTeleport", "atkBottomDash" };
        public string[] stage2 = new string[] { "atkBottomDash", "atkShootBomb", "atkFireStorm", "atkTeleport", "atkShootBomb" };
        public string[] stage3 = new string[] { "atkFireStorm", "atkFireStorm", "atkShootBomb", "atkBottomDash", "atkShootBomb" };
        public string[] stage4 = new string[] { "atkFireStorm", "atkShootBomb", "atkShootBomb" };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Terror");
            Main.npcFrameCount[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            velMax = 6;
            velAccel = .25f;
            targetOffset = new Vector2(0, -265);

            attackPointer = 0;
            attacks = stage0;

            npc.aiStyle = -1; // Will not have any AI from any existing AI styles. 
            npc.lifeMax = 3400; // The Max HP the boss has on Normal
            npc.damage = 31; // The base damage value the boss has on Normal
            npc.defense = 12; // The base defense on Normal
            npc.knockBackResist = 0f; // No knockback
            npc.width = 200;
            npc.height = 200;
            npc.value = 10000;
            npc.npcSlots = 1f; // The higher the number, the more NPC slots this NPC takes.
            npc.boss = true; // Is a boss
            npc.lavaImmune = false; // Not hurt by lava
            npc.noGravity = true; // Not affected by gravity
            npc.noTileCollide = true; // Will not collide with the tiles. 
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            music = MusicID.Boss1;
            bossBag = mod.ItemType("FlyingTerrorBag"); // Needed for the NPC to drop loot bag.
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * bossLifeScale);
            npc.defense = (int)(npc.defense + numPlayers);
        }
		public override void NPCLoot()
		{
            // Drop the correct drops
            if (Main.expertMode)
                npc.DropBossBags();
            else
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TerrorWing"), 2);

            // For settings if the boss has been downed
            SupernovaWorld.downedFlyingTerror = true;
        }

		bool init = false;
        public override void AI()
		{
            // Set target
            if (init == false)
            {
                attacks = stage0;
                npc.netUpdate = true;
                init = true;
            }

            // Attack
            Attack();

            // Handle despawning
            DespawnHandler();

            // Movement
            if (_move)
			{
                target = targetPlayer.Center;
                target.X += targetOffset.X;
                target.Y += targetOffset.Y;
            }

            Move();

            // Add light to the boss
            //Lighting.AddLight(npc.Center, 183, 0, 255);
        }
        #region Attacks
        public void atkTeleport()
		{
            npc.ai[0]++;
            if (npc.ai[0] == 40)
            {
                npc.position.X = (Main.player[npc.target].position.X + 450);
                npc.position.Y = (Main.player[npc.target].position.Y - 300);
            }
            else if (npc.ai[0] > 50 && npc.ai[0] < 150)
            {
                npc.ai[2]++;
                if (npc.ai[2] >= 50)
                {
                    Shoot();
                    npc.ai[2] = 0;
                }
            }
            else if (npc.ai[0] == 151)
            {
                npc.position.X = (Main.player[npc.target].position.X - 450);
                npc.position.Y = (Main.player[npc.target].position.Y - 300);
            }
            else if (npc.ai[0] > 150 && npc.ai[0] < 250)
            {
                npc.ai[2]++;
                if (npc.ai[2] >= 50)
                {
                    Shoot();
                    npc.ai[2] = 0;
                }
            }
            else if (npc.ai[0] >= 300)
            {
                npc.ai[0] = 0;
                attackPointer++;
            }
        }
        public void atkBomb()
		{
            npc.ai[0]++;
            if (npc.ai[0] == 50)
                TerrorBomb();
            else if (npc.ai[0] >= 100)
            {
                npc.ai[0] = 0;
                attackPointer++;
            }
        }
        public void atkFireStorm()
		{
            npc.ai[0]++;
            if (npc.ai[0] == 40)
            {
                ShootFlames();
            }
            else if (npc.ai[0] == 80)
            {
                ShootFlames();
            }
            else if (npc.ai[0] >= 120)
            {
                npc.ai[0] = 0;
                ShootFlames();
                attackPointer++;
            }
        }
        public void atkShootBomb()
		{
            npc.ai[0]++;
            if (npc.ai[0] == 35)
            {
                int type = mod.ProjectileType("TerrorBomb");
                Main.PlaySound(SoundID.Item20, npc.Center);
                Vector2 vector8 = new Vector2(npc.position.X + (npc.width - 50), npc.position.Y + (-npc.height + 200));

                float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.5f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.5f)));

                Projectile.NewProjectile(vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * 14)), (float)(-(Math.Sin(rotation) * 14)), type, largeAttackDamage, 0f, 0);
            }
            else if (npc.ai[0] >= 65)
            {
                npc.ai[0] = 0;
                attackPointer++;
            }
        }
        public void atkBottomDash()
		{
            _move = false;
            npc.ai[0]++;
            if(npc.ai[0] == 50)
			{
                target = targetPlayer.position;
                target.Y -= 500;
                npc.alpha = 95;

                for (int i = 0; i <= 20; i++)
                    Dust.NewDust(npc.Center, npc.width, npc.height, DustID.Shadowflame);
            }
            else if(npc.ai[0] == 90)
			{
                npc.position.X = (Main.player[npc.target].position.X);
                npc.position.Y = (Main.player[npc.target].position.Y + 650);
                target = new Vector2(target.X, target.Y - 1700);

                velMax *= 3;
                velAccel *= 3;
                npc.alpha = 0;
            }
            else if(npc.ai[0] >= 180)
			{
                velMax /= 3;
                velAccel /= 3;
                _move = true;
                npc.ai[0] = 0;
                attackPointer++;
			}
        }
        #endregion
        private void Shoot()
        {
            float Speed = 14;  //projectile speed
            int type = mod.ProjectileType("TerrorProj");
            Main.PlaySound(SoundID.Item20, npc.Center);
            Vector2 vector8 = new Vector2(npc.position.X + npc.width, npc.position.Y + (-npc.height + 200));

            float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.5f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.5f)));

            Projectile.NewProjectile(vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * Speed) * Main.rand.NextFloat(.9f, 1.1f)), (float)(-(Math.Sin(rotation) * Speed) * Main.rand.NextFloat(.9f, 1.1f)), type, smallAttackDamage, 0f, 0);
        }
        private void ShootFlames()
        {
            float Speed = 0.75f;  //projectile speed

            int damage = 19;  //projectile damage
            int type = 596;  //put your projectile
            Main.PlaySound(SoundID.Item20, (int)npc.position.X, (int)npc.position.Y);
            Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));

            float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.5f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.5f)));

            Projectile.NewProjectile(vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -4), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, 0);
            Projectile.NewProjectile(vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -2), (float)((Math.Sin(rotation) * Speed) * -2), type, damage, 0f, 0);
            Projectile.NewProjectile(vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, 0);

            npc.ai[1] = 0;
        }
        private void TerrorBomb()
        {
            int type = mod.ProjectileType("TerrorBomb");
            Main.PlaySound(SoundID.Item20, npc.Center);
            Projectile.NewProjectile(npc.position.X + 20, npc.position.Y + 20, 0, ShootDirection, type, largeAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
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
            if(velMagnitude < velMax && velMagnitude < targetVel)
                velMagnitude += velAccel;
            
            if (velMagnitude > targetVel)
                velMagnitude -= velAccel;

            // Make sure we don't devide by 0
            if(dist != 0)
                // Move to 'target'
                npc.velocity = npc.DirectionTo(target) * velMagnitude;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 1;
            npc.frameCounter %= 20;
            int frame = (int)(npc.frameCounter / 6.7);
            if (frame >= Main.npcFrameCount[npc.type]) frame = 0;
            npc.frame.Y = frame * frameHeight;
        }
        private void DespawnHandler()
        {
            if (!targetPlayer.active || targetPlayer.dead || Main.dayTime)
            {
                npc.TargetClosest(false);
                targetPlayer = Main.player[npc.target];
                if (!targetPlayer.active || targetPlayer.dead || Main.dayTime)
                {
                    npc.velocity = new Vector2(0f, -10f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
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
            // Stages
            if (npc.life <= (npc.lifeMax * .3f))
                attacks = stage4;

            else if (npc.life <= (npc.lifeMax * 50f))
                attacks = stage3;

            else if (npc.life <= (npc.lifeMax * .70))
                attacks = stage2;

            else if (npc.life <= (npc.lifeMax * .9f))
                attacks = stage1;

            // Reset attack pointer when we have done all the attacks for this stage
            if (attackPointer >= attacks.Length) attackPointer = 0;

            return true;
        }
    }
}
