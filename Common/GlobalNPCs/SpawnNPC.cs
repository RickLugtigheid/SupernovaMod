using Newtonsoft.Json;
using SupernovaMod.Common.Players;
using SupernovaMod.Content.Npcs.HorrorSpace;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Common.GlobalNPCs
{
	public class SpawnNPC : GlobalNPC
	{
		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			Main.NewText("ZoneHorrorSpace: " + spawnInfo.Player.Supernova().ZoneHorrorSpace);
			if (spawnInfo.Player.Supernova().ZoneHorrorSpace)
			{
				Main.NewText(JsonConvert.SerializeObject(pool));
				SetSpawnPoolHorrorSpace(pool, spawnInfo);
				Main.NewText(JsonConvert.SerializeObject(pool));
			}
		}

		private void SetSpawnPoolHorrorSpace(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			// Clear the old pool
			pool.Clear();

			// ======================================
			// Add all Horror Space NPCs to the pool,
			// if thier required conditions are met.
			// ======================================

			if (Main.dayTime)
			{
				pool.Add(ModContent.NPCType<EldritchSlime>(), .3f);
			}
		}
	}
}
