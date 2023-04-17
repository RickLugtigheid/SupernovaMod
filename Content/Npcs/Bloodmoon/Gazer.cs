using Microsoft.Xna.Framework;
using SupernovaMod.Common;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SupernovaMod.Content.Npcs.Bloodmoon
{
    public class Gazer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gazer");
            Main.npcFrameCount[NPC.type] = 7;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                PortraitPositionYOverride = -20,
                // Influences how the NPC looks in the Bestiary
                Velocity = 1 // Draws the NPC in the bestiary as if its walking +1 tiles in the x directions
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Impish terrors to all excavators beneath the surface. These unique creatures have evolved and adapted to coexist with the harsh circumstances of the underground. But something's hinting towards them being more than just lifeforms..."),
            });
        }

        int timer;
        int ShootDamage;
        int shootTimer;
        public override void SetDefaults()
        {
            // Change stats when in Hardmode
            //
            if (Main.hardMode)
            {
                NPC.lifeMax = 175;
                NPC.defense = 35;
                ShootDamage = 30;
                shootTimer = 80;
            }
            else
            {
                NPC.lifeMax = 60;
                NPC.defense = 12;
                ShootDamage = 12;
                shootTimer = 120;
            }
            NPC.width = 44;
            NPC.height = 44;
            NPC.damage = 20;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 44;
            AIType = NPCID.Harpy;  //npc behavior
            AnimationType = NPCID.Harpy;
            NPC.HitSound = SoundID.NPCHit9;
            NPC.DeathSound = SoundID.NPCDeath11;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter -= .8F; // Determines the animation speed. Higher value = faster animation.
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;

            NPC.spriteDirection = NPC.direction;
        }

        public override void AI()
        {
            int radius = 625;
            if (!NPC.confused && Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) <= radius)
            {
                timer++;
                if (timer >= shootTimer)
                {
                    Shoot();
                    timer = 0;
                }
            }
        }

        void Shoot()
        {
            int type = ProjectileID.RayGunnerLaser;// 438 || 100
            Vector2 Velocity = Mathf.VelocityFPTP(NPC.Center, new Vector2(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y), 5);
            int Spread = 1;
            float SpreadMult = 0.15f;
            Velocity.X = Velocity.X + Main.rand.Next(-Spread, Spread + 1) * SpreadMult;
            Velocity.Y = Velocity.Y + Main.rand.Next(-Spread, Spread + 1) * SpreadMult;
            int i = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, Velocity.X, Velocity.Y, type, ShootDamage, 1.75f);
            Main.projectile[i].hostile = true;
            Main.projectile[i].friendly = true;
            Main.projectile[i].tileCollide = true;
        }

        //public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.Cavern.Chance * 0.02f;
    }
}