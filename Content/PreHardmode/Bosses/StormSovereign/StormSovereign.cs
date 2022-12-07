using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Supernova.Common.Systems;
using Supernova.Api;
using Terraria.GameContent.Bestiary;

namespace Supernova.Content.PreHardmode.Bosses.StormSovereign
{
    [AutoloadBossHead]
    public class StormSovereign : ModNPC
    {
        public bool noAI = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm Sovereign TEST");
            Main.npcFrameCount[NPC.type] = 3;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                Scale = .5f,
                Velocity = 1,
                PortraitPositionYOverride = 8
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
				new FlavorTextBestiaryInfoElement("TODO."),
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = NPCAIStyleID.DemonEye; // Will not have any AI from any existing AI styles. 
            NPC.lifeMax = 4000; // The Max HP the boss has on Normal
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

        private float _warnTime;

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 1.1f);
            NPC.defense = (int)(NPC.defense + numPlayers);

            _warnTime = Main.masterMode ? 20 : Main.expertMode ? 40 : 80;
        }

        bool init = false;
        Player targetPlayer;
        public override void AI()
        {
            targetPlayer = Main.player[NPC.target];

            if (!init)
			{
                NPC.netUpdate = true;
                init = true;
			}

            NPC.ai[0]++;
            if (NPC.ai[0] >= 100)
			{
                SpawnLightningStrike();
                NPC.ai[0] = 0;
			}
        }

        public void SpawnLightningStrike()
		{
            int type = ModContent.ProjectileType<LightningStrike>();
            Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPlayer.Center + new Vector2(0, 10), Vector2.Zero, type, 30, 1, Main.myPlayer, _warnTime, 8);
        }
    }
}
