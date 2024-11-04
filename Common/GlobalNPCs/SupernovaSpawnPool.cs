using SupernovaMod.Content.Npcs.DreamlandsNPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

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

            //if (Main.dayTime)
            {
                pool.Add(ModContent.NPCType<EldritchSlime>(), .3f);
            }
        }
    }
}
