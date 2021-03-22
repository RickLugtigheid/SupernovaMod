using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Supernova.Npcs.PreHardmode
{
    public class Gazer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gazer");
            Main.npcFrameCount[npc.type] = 7;
        }
        int timer;
        int ShootDamage;
        int shootTimer;
        public override void SetDefaults()
        {
            if (Main.hardMode == true)
            {
                npc.lifeMax = 175;
                npc.defense = 35;
                ShootDamage = 30;
                shootTimer = 80;
            }
            else
            {
                npc.lifeMax = 60;
                npc.defense = 12;
                ShootDamage = 12;
                shootTimer = 120;
            }
            npc.width = 44;
            npc.height = 44;
            npc.damage = 20;
            npc.value = 60f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 44;
            aiType = NPCID.Harpy;  //npc behavior
            animationType = NPCID.Harpy;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter -= .8F; // Determines the animation speed. Higher value = faster animation.
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;

            npc.spriteDirection = npc.direction;
        }

        public override void AI()
        {
            int radius = 625;
            if (Vector2.Distance(Main.player[npc.target].Center, npc.Center) <= radius)
            {
                timer++;
                if(timer >= shootTimer)
                {
                    Shoot();
                    timer = 0;
                }
            }
        }

        void Shoot()
        {
            int type = 438;//100
            Vector2 Velocity = Calc.VelocityFPTP(npc.Center, new Vector2(Main.player[npc.target].Center.X, Main.player[npc.target].Center.Y), 5);
            int Spread = 1;
            float SpreadMult = 0.15f;
            Velocity.X = Velocity.X + Main.rand.Next(-Spread, Spread + 1) * SpreadMult;
            Velocity.Y = Velocity.Y + Main.rand.Next(-Spread, Spread + 1) * SpreadMult;
            int i = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Velocity.X, Velocity.Y, type, ShootDamage, 1.75f);
            Main.projectile[i].hostile = true;
            Main.projectile[i].friendly = true;
            Main.projectile[i].tileCollide = false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => (!spawnInfo.lihzahrd && !spawnInfo.invasion) && spawnInfo.player.ZoneRockLayerHeight ? 0.031f : 0;
    }
}