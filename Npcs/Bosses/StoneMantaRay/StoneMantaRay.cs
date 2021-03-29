using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace Supernova.Npcs.Bosses.StoneMantaRay
{
	[AutoloadBossHead]
	public class StoneMantaRay : Boss
    {
        public bool noAI = false;

        /* Stats */
        public int smallAttackDamage = 30;
        const float ShootKnockback = 5f;
        const int ShootDirection = 7;

        /* Stage Attacks */
        public string[] stage0 = new string[] { "atkSpear", "atkSpear" };
        public string[] stage1 = new string[] { "atkSpear", "atkSpear", "atkSpearRain" };
        public string[] stage2 = new string[] { "atkSpear", "atkSummon", "atkSpear", "atkSpear", "atkSpearRain" };
        public string[] stage3 = new string[] { "atkSpearFast", "atkSpearRain", "atkSpear", "atkSpearFast", "atkSpearFast" };


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Manta Ray");
            Main.npcFrameCount[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1; // Will not have any AI from any existing AI styles. 
            npc.lifeMax = 4500; // The Max HP the boss has on Normal
            npc.damage = 30; // The base damage value the boss has on Normal
            npc.defense = 8; // The base defense on Normal
            npc.knockBackResist = 0f; // No knockback
            npc.width = 150;
            npc.height = 150;
            npc.value = 10000;
            npc.npcSlots = 1f; // The higher the number, the more NPC slots this NPC takes.
            npc.boss = true; // Is a boss
            npc.lavaImmune = false; // Not hurt by lava
            npc.noGravity = true; // Not affected by gravity
            npc.noTileCollide = true; // Will not collide with the tiles. 
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            music = MusicID.Boss1;
            bossBag = mod.ItemType("StoneMantaRayBag"); // Needed for the NPC to drop loot bag.
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
            //if (Main.expertMode)
            //    npc.DropBossBags();

            for (int i = 0; i < Main.rand.Next(1, 2); i++)
                switch (Main.rand.Next(2))
                {
                    case 0:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SurgestoneSword"));
                        break;
                    case 1:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StoneRepeater"));
                        break;
                    default:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StoneGlove"));
                        break;
                }

            // For settings if the boss has been downed
            SupernovaWorld.downedStoneManta = true;
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
            int minTilePosX = (int)(npc.position.X / 16.0) - 1;
            int maxTilePosX = (int)((npc.position.X + npc.width) / 16.0) + 2;
            int minTilePosY = (int)(npc.position.Y / 16.0) - 1;
            int maxTilePosY = (int)((npc.position.Y + npc.height) / 16.0) + 2;
            if (minTilePosX < 0)
                minTilePosX = 0;
            if (maxTilePosX > Main.maxTilesX)
                maxTilePosX = Main.maxTilesX;
            if (minTilePosY < 0)
                minTilePosY = 0;
            if (maxTilePosY > Main.maxTilesY)
                maxTilePosY = Main.maxTilesY;

            bool collision = false;
            // This is the initial check for collision with tiles.
            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    if (Main.tile[i, j] != null && (Main.tile[i, j].nactive() && (Main.tileSolid[(int)Main.tile[i, j].type] || Main.tileSolidTop[(int)Main.tile[i, j].type] && (int)Main.tile[i, j].frameY == 0) || (int)Main.tile[i, j].liquid > 64))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(i * 16);
                        vector2.Y = (float)(j * 16);
                        if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16.0 && (npc.position.Y + npc.height > (double)vector2.Y && npc.position.Y < vector2.Y + 16.0))
                        {
                            collision = true;
                            if (Main.rand.Next(100) == 0 && Main.tile[i, j].nactive())
                                WorldGen.KillTile(i, j, true, true, false);
                        }
                    }
                }
            }
            // If there is no collision with tiles, we check if the distance between this NPC and its target is too large, so that we can still trigger 'collision'.
            if (!collision)
            {
                Rectangle rectangle1 = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                int maxDistance = 1000;
                bool playerCollision = true;
                for (int index = 0; index < 255; ++index)
                {
                    if (Main.player[index].active)
                    {
                        Rectangle rectangle2 = new Rectangle((int)Main.player[index].position.X - maxDistance, (int)Main.player[index].position.Y - maxDistance, maxDistance * 2, maxDistance * 2);
                        if (rectangle1.Intersects(rectangle2))
                        {
                            playerCollision = false;
                            break;
                        }
                    }
                }
                if (playerCollision)
                    collision = true;
            }

            // speed determines the max speed at which this NPC can move.
            // Higher value = faster speed.
            float speed = 9f;
            // acceleration is exactly what it sounds like. The speed at which this NPC accelerates.
            float acceleration = 0.1f;

            Vector2 npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float targetXPos = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2);
            float targetYPos = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2);

            float targetRoundedPosX = (float)((int)(targetXPos / 16.0) * 16);
            float targetRoundedPosY = (float)((int)(targetYPos / 16.0) * 16);
            npcCenter.X = (float)((int)(npcCenter.X / 16.0) * 16);
            npcCenter.Y = (float)((int)(npcCenter.Y / 16.0) * 16);
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            // If we do not have any type of collision, we want the NPC to fall down and de-accelerate along the X axis.
            if (!collision)
            {
                npc.TargetClosest(true);
                npc.velocity.Y = npc.velocity.Y + 0.11f;
                if (npc.velocity.Y > speed)
                    npc.velocity.Y = speed;
                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.4)
                {
                    if (npc.velocity.X < 0.0)
                        npc.velocity.X = npc.velocity.X - acceleration * 1.1f;
                    else
                        npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
                }
                else if (npc.velocity.Y == speed)
                {
                    if (npc.velocity.X < dirX)
                        npc.velocity.X = npc.velocity.X + acceleration;
                    else if (npc.velocity.X > dirX)
                        npc.velocity.X = npc.velocity.X - acceleration;
                }
                else if (npc.velocity.Y > 4.0)
                {
                    if (npc.velocity.X < 0.0)
                        npc.velocity.X = npc.velocity.X + acceleration * 0.9f;
                    else
                        npc.velocity.X = npc.velocity.X - acceleration * 0.9f;
                }
            }
            // Else we want to play some audio (soundDelay) and move towards our target.
            else
            {
                if (npc.soundDelay == 0)
                {
                    float num1 = length / 40f;
                    if (num1 < 10.0)
                        num1 = 10f;
                    if (num1 > 20.0)
                        num1 = 20f;
                    npc.soundDelay = (int)num1;
                    Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 1);
                }
                float absDirX = Math.Abs(dirX);
                float absDirY = Math.Abs(dirY);
                float newSpeed = speed / length;
                dirX = dirX * newSpeed;
                dirY = dirY * newSpeed;
                if (npc.velocity.X > 0.0 && dirX > 0.0 || npc.velocity.X < 0.0 && dirX < 0.0 || (npc.velocity.Y > 0.0 && dirY > 0.0 || npc.velocity.Y < 0.0 && dirY < 0.0))
                {
                    if (npc.velocity.X < dirX)
                        npc.velocity.X = npc.velocity.X + acceleration;
                    else if (npc.velocity.X > dirX)
                        npc.velocity.X = npc.velocity.X - acceleration;
                    if (npc.velocity.Y < dirY)
                        npc.velocity.Y = npc.velocity.Y + acceleration;
                    else if (npc.velocity.Y > dirY)
                        npc.velocity.Y = npc.velocity.Y - acceleration;
                    if (Math.Abs(dirY) < speed * 0.2 && (npc.velocity.X > 0.0 && dirX < 0.0 || npc.velocity.X < 0.0 && dirX > 0.0))
                    {
                        if (npc.velocity.Y > 0.0)
                            npc.velocity.Y = npc.velocity.Y + acceleration * 2f;
                        else
                            npc.velocity.Y = npc.velocity.Y - acceleration * 2f;
                    }
                    if (Math.Abs(dirX) < speed * 0.2 && (npc.velocity.Y > 0.0 && dirY < 0.0 || npc.velocity.Y < 0.0 && dirY > 0.0))
                    {
                        if (npc.velocity.X > 0.0)
                            npc.velocity.X = npc.velocity.X + acceleration * 2f;
                        else
                            npc.velocity.X = npc.velocity.X - acceleration * 2f;
                    }
                }
                else if (absDirX > absDirY)
                {
                    if (npc.velocity.X < dirX)
                        npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
                    else if (npc.velocity.X > dirX)
                        npc.velocity.X = npc.velocity.X - acceleration * 1.1f;
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                    {
                        if (npc.velocity.Y > 0.0)
                            npc.velocity.Y = npc.velocity.Y + acceleration;
                        else
                            npc.velocity.Y = npc.velocity.Y - acceleration;
                    }
                }
                else
                {
                    if (npc.velocity.Y < dirY)
                        npc.velocity.Y = npc.velocity.Y + acceleration * 1.1f;
                    else if (npc.velocity.Y > dirY)
                        npc.velocity.Y = npc.velocity.Y - acceleration * 1.1f;
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                    {
                        if (npc.velocity.X > 0.0)
                            npc.velocity.X = npc.velocity.X + acceleration;
                        else
                            npc.velocity.X = npc.velocity.X - acceleration;
                    }
                }
            }
            // Set the correct rotation for this NPC.
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;

            // Some netupdate stuff (multiplayer compatibility).
            if (collision)
            {
                if (npc.localAI[0] != 1)
                    npc.netUpdate = true;
                npc.localAI[0] = 1f;
            }
            else
            {
                if (npc.localAI[0] != 0.0)
                    npc.netUpdate = true;
                npc.localAI[0] = 0.0f;
            }
            if ((npc.velocity.X > 0.0 && npc.oldVelocity.X < 0.0 || npc.velocity.X < 0.0 && npc.oldVelocity.X > 0.0 || (npc.velocity.Y > 0.0 && npc.oldVelocity.Y < 0.0 || npc.velocity.Y < 0.0 && npc.oldVelocity.Y > 0.0)) && !npc.justHit)
                npc.netUpdate = true;

            // Add light to the boss
            //Lighting.AddLight(npc.Center, 183, 0, 255);
        }
        #region Attacks
        public void atkSpear()
        {
            npc.ai[1]++;
            if(npc.ai[1] == 60)
			{
                float Speed = 12;

                int type = mod.ProjectileType("StoneSpear");
                Main.PlaySound(SoundID.Item20, npc.Center); // Boing
                Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 4), npc.position.Y + (-npc.height + 200));

                float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.2f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.2f)));

                Projectile.NewProjectile(vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * Speed)), (float)(-(Math.Sin(rotation) * Speed)), type, (int)(smallAttackDamage * .75f), 0f, 0);
            }
            else if(npc.ai[1] >= 120)
			{
                npc.ai[1] = 0;
                attackPointer++;
            }
        }
        public void atkSpearRain()
		{
            npc.ai[1]++;
            int type = mod.ProjectileType("StoneSpear");
            if(npc.ai[1] == 70)
                Projectile.NewProjectile(targetPlayer.position.X + 150, targetPlayer.position.Y - 900, 0, ShootDirection, type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            else if (npc.ai[1] == 80)
                Projectile.NewProjectile(targetPlayer.position.X - 150, targetPlayer.position.Y - 900, 0, ShootDirection, type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            else if (npc.ai[1] == 90)
                Projectile.NewProjectile(targetPlayer.position.X + 400, targetPlayer.position.Y - 1000, 0, ShootDirection, type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            else if (npc.ai[1] == 100)
                Projectile.NewProjectile(targetPlayer.position.X - 400, targetPlayer.position.Y - 1000, 0, ShootDirection, type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            else if (npc.ai[1] == 110)
                Projectile.NewProjectile(targetPlayer.position.X + 600, targetPlayer.position.Y - 1100, 0, ShootDirection, type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            else if (npc.ai[1] == 120)
                Projectile.NewProjectile(targetPlayer.position.X - 600, targetPlayer.position.Y - 1100, 0, ShootDirection, type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            else if (npc.ai[1] == 140)
			{
                npc.ai[1] = 0;
                attackPointer++;
            }
        }
        public void atkSummon()
		{
            npc.ai[1]++;
            if(npc.ai[1] == 80)
                NPC.NewNPC((int)npc.position.X + Main.rand.Next(-75, 75), (int)npc.position.Y + Main.rand.Next(-75, 75), mod.NPCType("StoneRayChild"));
            else if(npc.ai[1] >= 150)
			{
                npc.ai[1] = 0;
                attackPointer++;
            }
        }
        public void atkSpearFast()
        {
            npc.ai[1]++;
            if (npc.ai[1] == 40)
            {
                float Speed = 13;

                int type = mod.ProjectileType("StoneSpear");
                Main.PlaySound(SoundID.Item20, npc.Center); // Boing
                Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 4), npc.position.Y + (-npc.height + 200));

                float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.2f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.2f)));

                Projectile.NewProjectile(vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * Speed)), (float)(-(Math.Sin(rotation) * Speed)), type, (int)(smallAttackDamage * .75f), 0f, 0);
            }
            else if (npc.ai[1] >= 80)
            {
                npc.ai[1] = 0;
                attackPointer++;
            }
        }
        #endregion

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
            if (!targetPlayer.active || targetPlayer.dead)
            {
                npc.TargetClosest(false);
                targetPlayer = Main.player[npc.target];
                if (!targetPlayer.active || targetPlayer.dead)
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
                attacks = stage3;

            else if (npc.life <= (npc.lifeMax / 2.25))
                attacks = stage2;

            else if (npc.life <= (npc.lifeMax * 0.86f))
                attacks = stage1;

            // Reset attack pointer when we have done all the attacks for this stage
            if (attackPointer >= attacks.Length) attackPointer = 0;
            return true;
        }
    }
}
