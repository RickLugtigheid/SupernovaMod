using SupernovaMod.Content.Npcs.DreamlandsNPCs;
using System.Collections;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SupernovaMod.Common.GlobalNPCs
{
    public class SupernovaSpawnPool : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            Main.NewText("ZoneDreamlands: " + spawnInfo.Player.Supernova().ZoneDreamlands);
            if (spawnInfo.Player.Supernova().ZoneDreamlands)
            {
                RegisterSpawnPoolDreamlands(pool, spawnInfo);
            }
        }

        private void RegisterSpawnPoolDreamlands(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            // Clear the vanilla pool
            pool.Clear();

            // ==================================== //
            // Add all Dreamlands NPCs to the pool  //
            // when thier other required conditions //
            // are met.                             //
            // ==================================== //

            // Overworld NPCs
            //
            if (spawnInfo.Player.ZoneOverworldHeight)
            {
                if (Main.dayTime)
                {
                    pool.Add(ModContent.NPCType<EldritchSlime>(), .3f);
                }
                else
                {
                    pool.Add(NPCID.DemonEye, .2f);
                    pool.Add(190, .1f);
                    pool.Add(191, .1f);
                    pool.Add(192, .1f);
                    pool.Add(193, .1f);
                    pool.Add(194, .1f);
                    pool.Add(317, .05f);
                    pool.Add(318, .05f);

                    if (Main.hardMode)
                    {
                        pool.Add(NPCID.WanderingEye, .1f);
                    }
                }
            }
            // Underground NPCs
            //
            else
            {

            }
        }
    }
}
