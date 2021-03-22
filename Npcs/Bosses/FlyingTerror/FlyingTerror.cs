using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace Supernova.Npcs.Bosses.FlyingTerror
{
	[AutoloadBossHead]
	public class FlyingTerror : Boss
    {
        private float speed;

        public bool _move = true;

        /* Stats */
        public int smallAttackDamage = 19;
        public int largeAttackDamage = 32;
        const float ShootKnockback = 5f;
        const int ShootDirection = 7;

        /* Stage Attacks */
        public string[] stage0 = new string[] { "atkTeleport", "atkBomb" };
        public string[] stage1 = new string[] { "atkTeleport", "atkBomb", "atkTeleport", "atkFireStorm" };
        public string[] stage2 = new string[] { "atkTeleport", "atkShootBomb", "atkFireStorm", "atkTeleport", "atkShootBomb" };
        public string[] stage3 = new string[] { "atkFireStorm", "atkFireStorm", "atkBomb", "atkTeleport", "atkBomb" };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Terror");
            Main.npcFrameCount[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1; // Will not have any AI from any existing AI styles. 
            npc.lifeMax = 3200; // The Max HP the boss has on Normal
            npc.damage = 23; // The base damage value the boss has on Normal
            npc.defense = 11; // The base defense on Normal
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
            npc.damage = (int)(npc.damage * 1.2f);
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
                Move(new Vector2(-0, -Main.rand.Next(180, 300)));
            else
                Move(new Vector2(-0, -0f));

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
            {
                TerrorBomb();
            }
            else if (npc.ai[0] == 100)
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
            else if (npc.ai[0] == 120)
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
            else if (npc.ai[0] == 65)
            {
                npc.ai[0] = 0;
                attackPointer++;
            }
        }
        #endregion
        private void Shoot()
        {
            float Speed = 17;  //projectile speed
            int type = mod.ProjectileType("TerrorProj");
            Main.PlaySound(SoundID.Item20, npc.Center);
            Vector2 vector8 = new Vector2(npc.position.X + (npc.width - 50), npc.position.Y + (-npc.height + 200));

            float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.5f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.5f)));

            Projectile.NewProjectile(vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * Speed)), (float)(-(Math.Sin(rotation) * Speed)), type, smallAttackDamage, 0f, 0);
        }
        private void ShootFlames()
        {
            float Speed = 0.65f;  //projectile speed

            int damage = 21;  //projectile damage
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

        private void Move(Vector2 offset)
        {
            if (Main.netMode != 1)
            {
                Vector2 moveTo = targetPlayer.Center + offset; // Gets the point that the npc will be moving to.
                Vector2 move = moveTo - npc.Center;

                float magnitude = Magnitude(move);
                if (magnitude > speed)
                {
                    move *= speed / magnitude;
                }
                float turnResistance = 21f; // The larget the number the slower the npc will turn.
                move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
                magnitude = Magnitude(move);
                if (magnitude > speed)
                {
                    move *= speed / magnitude;
                }
                npc.velocity = move;
                if (_move == true)
                {
                    speed = 6f; // Sets the max speed of the npc.
                }
                else
                { speed = 0.001f; }
            }
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
            if (npc.life <= (npc.lifeMax * .25f))
                attacks = stage3;

            else if (npc.life <= (npc.lifeMax / 2))
                attacks = stage2;

            else if (npc.life <= (npc.lifeMax * 0.9f))
                attacks = stage1;

            // Reset attack pointer when we have done all the attacks for this stage
            if (attackPointer >= attacks.Length) attackPointer = 0;
            return true;
        }
    }
}
