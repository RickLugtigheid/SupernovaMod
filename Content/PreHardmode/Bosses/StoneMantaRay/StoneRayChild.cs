using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Bosses.StoneMantaRay
{
    public class StoneRayChild : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("StoneRayChild");
            Main.npcFrameCount[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 44;
            NPC.damage = 40;
            NPC.defense = 14;
            NPC.lifeMax = 50;
            NPC.value = 600f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 0;
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = true; // Will not collide with the tiles. 
            //aiType = NPCID.Harpy;  //npc behavior
            AnimationType = NPCID.Harpy;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        bool attackRun = false;
        float speed = .45f;
        public override void AI()
        {
            float dist = MathHelper.Distance(NPC.position.X, Main.player[NPC.target].position.X);
            float targetHight = attackRun ? MathHelper.Clamp(dist, 170, 300) : 300;


            Vector2 vector444 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
            float targetX = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector444.X;
            float targetY = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - targetHight - vector444.Y;

            // If not in an attack run we try to get to a 900+ distance from the player
            if (!attackRun)
            {
                NPC.ai[0] = 0;
                targetX = -targetX;
                if (dist > 900)
				{
                    NPC.ai[1] = -NPC.spriteDirection;
                    attackRun = true;
                }

                if (NPC.velocity.X < targetX)
                {
                    NPC.velocity.X = NPC.velocity.X + speed;
                    NPC.direction = 1;
                    if (NPC.velocity.X < 0f && targetX > 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X + speed;
                    }
                }
                else if (NPC.velocity.X > targetX)
                {
                    NPC.velocity.X = NPC.velocity.X - speed;
                    NPC.direction = -1;
                    if (NPC.velocity.X > 0f && targetX < 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X - speed;
                    }
                }
            }
            else
			{
                NPC.ai[0] = -(dist * .25f);

                if (NPC.ai[1] == 1)
                {
                    NPC.velocity.X = NPC.velocity.X - speed;
                    NPC.direction = 1;
                }
                else if (NPC.ai[1] == -1)
                {
                    NPC.velocity.X = NPC.velocity.X + speed;
                    NPC.direction = -1;
                }

                // Shoot
                if (dist <= 200)
				{
                    if (NPC.ai[2] == 0)
                        Shoot();
                    NPC.ai[2] = 1;
                }

                // Check if we are done with this attack run
                if (
                    (NPC.ai[1] == 1 && (Main.player[NPC.target].position.X - 500) > NPC.position.X) ||
                    (NPC.ai[1] == -1 && (Main.player[NPC.target].position.X + 500) < NPC.position.X))
                {
                    attackRun = false;
                    NPC.ai[2] = 0;
                }
            }

            if (NPC.velocity.Y < targetY)
            {
                NPC.velocity.Y = NPC.velocity.Y + speed / 4;
                if (NPC.velocity.Y < 0f && targetY > 0f)
                {
                    NPC.velocity.Y = NPC.velocity.Y + speed / 4;
                }
            }
            else if (NPC.velocity.Y > targetY)
            {
                NPC.velocity.Y = NPC.velocity.Y - speed / 4;
                if (NPC.velocity.Y > 0f && targetY < 0f)
                {
                    NPC.velocity.Y = NPC.velocity.Y - speed / 4;
                }
            }
        }

        private void Shoot()
		{
            float Speed = 16;

            int type = ProjectileID.Turtle;
            SoundEngine.PlaySound(SoundID.Item20, NPC.Center); // Boing
            Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width / 4), NPC.position.Y + (NPC.height / 4));

            float rotation = (float)Math.Atan2(vector8.Y - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.2f)), vector8.X - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.2f)));

            Projectile.NewProjectile(NPC.GetSource_FromAI(), vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * Speed)), (float)(-(Math.Sin(rotation) * Speed)), type, 25, 0f, 0);
        }

		public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter -= .8F; // Determines the animation speed. Higher value = faster animation.
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;

            NPC.spriteDirection = NPC.direction;
        }
    }
}