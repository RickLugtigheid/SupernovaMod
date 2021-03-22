using System;
using Terraria.ModLoader;

namespace Supernova
{
	public class Supernova : Mod
	{
		/* Ring Code */
		public static ModHotKey ringAbilityButton;
		public override void Load()
		{
			// Registers a new hotkey
			ringAbilityButton = RegisterHotKey("Ring Ability", "Q");
		}

		/* Add Boss Checklist */
		public override void PostSetupContent()
		{
			/// [Terraria bosses]
			// SlimeKing = 1f;
			// EyeOfCthulhu = 2f;
			// EaterOfWorlds = 3f;
			// QueenBee = 4f;
			// Skeletron = 5f;
			// WallOfFlesh = 6f;
			// TheTwins = 7f;
			// TheDestroyer = 8f;
			// SkeletronPrime = 9f;
			// Plantera = 10f;
			// Golem = 11f;
			// DukeFishron = 12f;
			// LunaticCultist = 13f;
			// Moonlord = 14f;

			Mod bossChecklist = ModLoader.GetMod("BossChecklist");
			if (bossChecklist != null)
			{
				bossChecklist.Call("AddBossWithInfo", "Harbinger of Annihilation", 1.8f, (Func<bool>)(() => SupernovaWorld.downedHarbingerOfAnnihilation), "Kill a cosmic anomaly (spawns in the sky)");
				bossChecklist.Call("AddBossWithInfo", "Flying Terror", 3.7f, (Func<bool>)(() => SupernovaWorld.downedFlyingTerror), "Use a [i:" + ItemType("HorridChunk") + "] at night");
				bossChecklist.Call("AddBossWithInfo", "Stone MantaRay", 5.4f, (Func<bool>)(() => SupernovaWorld.downedStoneManta), "Use a [i:" + ItemType("MantaFood") + "] in the Underground stone layer");

				/*bossChecklist.Call("AddBossWithInfo", "Cosmic Collective", 6.9f, (Func<bool>)(() => SupernovaWorld.downedCosmicCollective), "Use a [i:" + ItemType("CosmicEgg") + "]");
				bossChecklist.Call("AddBossWithInfo", "Helios the Infernal Overlord", 8.4f, (Func<bool>)(() => SupernovaWorld.downedHelios), "Use a [i:" + ItemType("infernalRitualStone") + "] in hell");
				bossChecklist.Call("AddBossWithInfo", "Cocytus", 10.9f, (Func<bool>)(() => SupernovaWorld.downedCocytus), "Use a [i:" + ItemType("FrozenSkull") + "] at night in the snow biome");
				bossChecklist.Call("AddBossWithInfo", "Shimmering Light Metatron", 12.7f, (Func<bool>)(() => SupernovaWorld.downedShimmeringLightMetatron), "Use a [i:" + ItemType("UnholyPact") + "] at daytime in the hallowed biome");

				bossChecklist.Call("AddBossWithInfo", "Deathbringer", 15f, (Func<bool>)(() => SupernovaWorld.downedDeathbringer), "Use a [i:" + ItemType("HiTechBeacon") + "]");*/
			}
		}
	}
}