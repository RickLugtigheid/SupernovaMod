using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Supernova.Common.Systems;
using Supernova.Api.Core;

namespace Supernova.Content.PreHardmode.Bosses.StoneMantaRay
{
	[AutoloadBossHead]
	public class StoneMantaRay : ModBossNpc
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
        public string[] stage3 = new string[] { "atkSpearFast", "atkSpearRain", "atkSpear", "atkSpearFast", "atkSpearFast", "atkSummon" };


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Manta Ray Old");
            Main.npcFrameCount[NPC.type] = 3;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 
            NPC.lifeMax = 4500; // The Max HP the boss has on Normal
            NPC.damage = 34; // The base damage value the boss has on Normal
            NPC.defense = 10; // The base defense on Normal
            NPC.knockBackResist = 0f; // No knockback
            NPC.width = 150;
            NPC.height = 150;
            NPC.value = 10000;
            NPC.npcSlots = 1f; // The higher the number, the more NPC slots this NPC takes.
            NPC.boss = true; // Is a boss
            NPC.lavaImmune = false; // Not hurt by lava
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = true; // Will not collide with the tiles. 
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicID.Boss1;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 1.1f);
            NPC.defense = (int)(NPC.defense + numPlayers);
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
            // Add boss bag drop
            //npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<>()));

            for (int i = 0; i < Main.rand.Next(1, 2); i++)
			{
                switch (Main.rand.Next(2))
                {
                    case 0:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SurgestoneSword>()));
                        break;
                    case 1:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StoneRepeater>()));
                        break;
                    default:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StoneGlove>()));
                        break;
                }
            }

            SupernovaBosses.downedStormSovereign = true;
        }

		bool init = false;
        public override void AI()
		{
            // Set target
            if (init == false)
            {
                attacks = stage0;
                NPC.netUpdate = true;
                init = true;
            }
            if (Despawning()) return;

            // Attack
            Attack();

            WormAI();

            // Add light to the boss
            //Lighting.AddLight(npc.Center, 183, 0, 255);
        }
        #region Attacks
        public void atkSpear()
        {
            NPC.ai[1]++;
            if(NPC.ai[1] == 60)
			{
                float Speed = 18;

                int type = Mod.Find<ModProjectile>("StoneSpear").Type;
                SoundEngine.PlaySound(SoundID.Item20, NPC.Center); // Boing
                Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width / 4), NPC.position.Y + (-NPC.height + 200));

                float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.2f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.2f)));

                Projectile.NewProjectile(NPC.GetSource_FromAI(), vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * Speed)), (float)(-(Math.Sin(rotation) * Speed)), type, (int)(smallAttackDamage * .75f), 0f, 0);
            }
            else if(NPC.ai[1] >= 120)
			{
                NPC.ai[1] = 0;
                attackPointer++;
            }
        }
        public void atkSpearRain()
		{
            NPC.ai[1]++;
            int type = Mod.Find<ModProjectile>("StoneSpear").Type;
            if(NPC.ai[1] == 70)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPlayer.position.X, targetPlayer.position.Y - 900, 0, ShootDirection, type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            else if (NPC.ai[1] == 80)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPlayer.position.X - 100, targetPlayer.position.Y - 900, 0, ShootDirection, type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            else if (NPC.ai[1] == 90)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPlayer.position.X + 200, targetPlayer.position.Y - 1000, 0, ShootDirection, type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            else if (NPC.ai[1] == 100)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPlayer.position.X - 200, targetPlayer.position.Y - 1000, 0, ShootDirection, type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            else if (NPC.ai[1] == 110)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPlayer.position.X + 400, targetPlayer.position.Y - 1100, 0, ShootDirection, type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            else if (NPC.ai[1] == 120)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPlayer.position.X - 400, targetPlayer.position.Y - 1100, 0, ShootDirection, type, smallAttackDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            else if (NPC.ai[1] == 140)
			{
                NPC.ai[1] = 0;
                attackPointer++;
            }
        }
        public void atkSummon()
		{
            NPC.ai[1]++;
            if(NPC.ai[1] == 80)
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + Main.rand.Next(-75, 75), (int)NPC.position.Y + Main.rand.Next(-75, 75), Mod.Find<ModNPC>("StoneRayChild").Type);
            else if(NPC.ai[1] >= 150)
			{
                NPC.ai[1] = 0;
                attackPointer++;
            }
        }
        public void atkSpearFast()
        {
            NPC.ai[1]++;
            if (NPC.ai[1] == 40)
            {
                float Speed = 21;

                int type = Mod.Find<ModProjectile>("StoneSpear").Type;
                SoundEngine.PlaySound(SoundID.Item20, NPC.Center); // Boing
                Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width / 4), NPC.position.Y + (-NPC.height + 200));

                float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.2f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.2f)));

                Projectile.NewProjectile(NPC.GetSource_FromAI(), vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * Speed)), (float)(-(Math.Sin(rotation) * Speed)), type, (int)(smallAttackDamage * .75f), 0f, 0);
            }
            else if (NPC.ai[1] >= 80)
            {
                NPC.ai[1] = 0;
                attackPointer++;
            }
        }
        #endregion

        private int _maxDistance = 1300;
        // speed determines the max speed at which this NPC can move.
        // Higher value = faster speed.
        private float _speed = 9;
        // acceleration is exactly what it sounds like. The speed at which this NPC accelerates.
        private float _acceleration = 0.11f;
        public void WormAI()
		{
            int minTilePosX = (int)(NPC.position.X / 16.0) - 1;
            int maxTilePosX = (int)((NPC.position.X + NPC.width) / 16.0) + 2;
            int minTilePosY = (int)(NPC.position.Y / 16.0) - 1;
            int maxTilePosY = (int)((NPC.position.Y + NPC.height) / 16.0) + 2;
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
                    if (Main.tile[i, j] != null && (Main.tile[i, j].HasUnactuatedTile && (Main.tileSolid[(int)Main.tile[i, j].TileType] || Main.tileSolidTop[(int)Main.tile[i, j].TileType] && (int)Main.tile[i, j].TileFrameY == 0) || (int)Main.tile[i, j].LiquidAmount > 64))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(i * 16);
                        vector2.Y = (float)(j * 16);
                        if (NPC.position.X + NPC.width > vector2.X && NPC.position.X < vector2.X + 16.0 && (NPC.position.Y + NPC.height > (double)vector2.Y && NPC.position.Y < vector2.Y + 16.0))
                        {
                            collision = true;
                            if (Main.rand.Next(100) == 0 && Main.tile[i, j].HasUnactuatedTile)
                                WorldGen.KillTile(i, j, true, true, false);
                        }
                    }
                }
            }
            // If there is no collision with tiles, we check if the distance between this NPC and its target is too large, so that we can still trigger 'collision'.
            if (!collision)
            {
                Rectangle rectangle1 = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
                bool playerCollision = true;
                for (int index = 0; index < 255; ++index)
                {
                    if (Main.player[index].active)
                    {
                        Rectangle rectangle2 = new Rectangle((int)Main.player[index].position.X - _maxDistance, (int)Main.player[index].position.Y - _maxDistance, _maxDistance * 2, _maxDistance * 2);
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

            Vector2 npcCenter = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
            float targetXPos = Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2);
            float targetYPos = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2);

            float targetRoundedPosX = (float)((int)(targetXPos / 16.0) * 16);
            float targetRoundedPosY = (float)((int)(targetYPos / 16.0) * 16);
            npcCenter.X = ((int)(npcCenter.X / 16.0) * 16);
            npcCenter.Y = ((int)(npcCenter.Y / 16.0) * 16);
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            // If we do not have any type of collision, we want the NPC to fall down and de-accelerate along the X axis.
            if (!collision)
            {
                NPC.TargetClosest(true);
                NPC.velocity.Y = NPC.velocity.Y + 0.11f;
                if (NPC.velocity.Y > _speed)
                    NPC.velocity.Y = _speed;
                if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < _speed * 0.4)
                {
                    if (NPC.velocity.X < 0.0)
                        NPC.velocity.X = NPC.velocity.X - _acceleration * 1.1f;
                    else
                        NPC.velocity.X = NPC.velocity.X + _acceleration * 1.1f;
                }
                else if (NPC.velocity.Y == _speed)
                {
                    if (NPC.velocity.X < dirX)
                        NPC.velocity.X = NPC.velocity.X + _acceleration;
                    else if (NPC.velocity.X > dirX)
                        NPC.velocity.X = NPC.velocity.X - _acceleration;
                }
                else if (NPC.velocity.Y > 4.0)
                {
                    if (NPC.velocity.X < 0.0)
                        NPC.velocity.X = NPC.velocity.X + _acceleration * 0.9f;
                    else
                        NPC.velocity.X = NPC.velocity.X - _acceleration * 0.9f;
                }
            }
            // Else we want to play some audio (soundDelay) and move towards our target.
            else
            {
                if (NPC.soundDelay == 0)
                {
                    float num1 = length / 40f;
                    if (num1 < 10.0)
                        num1 = 10f;
                    if (num1 > 20.0)
                        num1 = 20f;
                    NPC.soundDelay = (int)num1;
                    SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                }
                float absDirX = Math.Abs(dirX);
                float absDirY = Math.Abs(dirY);
                float newSpeed = _speed / length;
                dirX = dirX * newSpeed;
                dirY = dirY * newSpeed;
                if (NPC.velocity.X > 0.0 && dirX > 0.0 || NPC.velocity.X < 0.0 && dirX < 0.0 || (NPC.velocity.Y > 0.0 && dirY > 0.0 || NPC.velocity.Y < 0.0 && dirY < 0.0))
                {
                    if (NPC.velocity.X < dirX)
                        NPC.velocity.X = NPC.velocity.X + _acceleration;
                    else if (NPC.velocity.X > dirX)
                        NPC.velocity.X = NPC.velocity.X - _acceleration;
                    if (NPC.velocity.Y < dirY)
                        NPC.velocity.Y = NPC.velocity.Y + _acceleration;
                    else if (NPC.velocity.Y > dirY)
                        NPC.velocity.Y = NPC.velocity.Y - _acceleration;
                    if (Math.Abs(dirY) < _speed * 0.2 && (NPC.velocity.X > 0.0 && dirX < 0.0 || NPC.velocity.X < 0.0 && dirX > 0.0))
                    {
                        if (NPC.velocity.Y > 0.0)
                            NPC.velocity.Y = NPC.velocity.Y + _acceleration * 2f;
                        else
                            NPC.velocity.Y = NPC.velocity.Y - _acceleration * 2f;
                    }
                    if (Math.Abs(dirX) < _speed * 0.2 && (NPC.velocity.Y > 0.0 && dirY < 0.0 || NPC.velocity.Y < 0.0 && dirY > 0.0))
                    {
                        if (NPC.velocity.X > 0.0)
                            NPC.velocity.X = NPC.velocity.X + _acceleration * 2f;
                        else
                            NPC.velocity.X = NPC.velocity.X - _acceleration * 2f;
                    }
                }
                else if (absDirX > absDirY)
                {
                    if (NPC.velocity.X < dirX)
                        NPC.velocity.X = NPC.velocity.X + _acceleration * 1.1f;
                    else if (NPC.velocity.X > dirX)
                        NPC.velocity.X = NPC.velocity.X - _acceleration * 1.1f;
                    if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < _speed * 0.5)
                    {
                        if (NPC.velocity.Y > 0.0)
                            NPC.velocity.Y = NPC.velocity.Y + _acceleration;
                        else
                            NPC.velocity.Y = NPC.velocity.Y - _acceleration;
                    }
                }
                else
                {
                    if (NPC.velocity.Y < dirY)
                        NPC.velocity.Y = NPC.velocity.Y + _acceleration * 1.1f;
                    else if (NPC.velocity.Y > dirY)
                        NPC.velocity.Y = NPC.velocity.Y - _acceleration * 1.1f;
                    if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < _speed * 0.5)
                    {
                        if (NPC.velocity.X > 0.0)
                            NPC.velocity.X = NPC.velocity.X + _acceleration;
                        else
                            NPC.velocity.X = NPC.velocity.X - _acceleration;
                    }
                }
            }
            // Set the correct rotation for this NPC.
            NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 1.57f;

            // Some netupdate stuff (multiplayer compatibility).
            if (collision)
            {
                if (NPC.localAI[0] != 1)
                    NPC.netUpdate = true;
                NPC.localAI[0] = 1f;
            }
            else
            {
                if (NPC.localAI[0] != 0.0)
                    NPC.netUpdate = true;
                NPC.localAI[0] = 0.0f;
            }
            if ((NPC.velocity.X > 0.0 && NPC.oldVelocity.X < 0.0 || NPC.velocity.X < 0.0 && NPC.oldVelocity.X > 0.0 || (NPC.velocity.Y > 0.0 && NPC.oldVelocity.Y < 0.0 || NPC.velocity.Y < 0.0 && NPC.oldVelocity.Y > 0.0)) && !NPC.justHit)
                NPC.netUpdate = true;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1;
            NPC.frameCounter %= 20;
            int frame = (int)(NPC.frameCounter / 6.7);
            if (frame >= Main.npcFrameCount[NPC.type]) frame = 0;
            NPC.frame.Y = frame * frameHeight;
        }
        private bool Despawning()
        {
            if (targetPlayer == null || !targetPlayer.active || targetPlayer.dead || !targetPlayer.ZoneRockLayerHeight)
            {
                NPC.TargetClosest(false);
                targetPlayer = Main.player[NPC.target];
                if (!targetPlayer.active || targetPlayer.dead || !targetPlayer.ZoneRockLayerHeight)
                {
                    NPC.velocity.Y = 10;
                    if (NPC.timeLeft > 120)
                        NPC.timeLeft = 120;
                    return true;
                }
            }
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Stages
            if (NPC.life <= (NPC.lifeMax * .3f))
                attacks = stage3;

            else if (NPC.life <= (NPC.lifeMax / 2.25))
                attacks = stage2;

            else if (NPC.life <= (NPC.lifeMax * 0.86f))
                attacks = stage1;

            // Reset attack pointer when we have done all the attacks for this stage
            if (attackPointer >= attacks.Length) attackPointer = 0;
            return true;
        }
    }
}
