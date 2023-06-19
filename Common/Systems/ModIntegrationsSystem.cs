using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems
{
	public class ModIntegrationsSystem : ModSystem
	{
		public override void PostSetupContent()
		{
			HandleIntegrationBossChecklist();
		}

		private void HandleIntegrationBossChecklist()
		{
			// Get the boss checklist mod object
			//
			if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
			{
				return; // Can not intergrate mod that is not loader
			}

			// [Pre-Harmode bosses]
			//
			new BossChecklistItemBuilder()
				.ForBoss(ModContent.NPCType<Content.Npcs.HarbingerOfAnnihilation.HarbingerOfAnnihilation>(), "Harbinger of Annihilation")
				.SetWeight(VanillaWeights.EyeOfCthulhu + .5f)   // After the Eye of Cthulhu
				.SetSpawnInfo("Kill a Cosmic Anomaly in Space")
				.SetDownedCallback(() => DownedSystem.downedHarbingerOfAnnihilation)
				.AddBoss(Mod, bossChecklistMod);

			new BossChecklistItemBuilder()
				.ForBoss(ModContent.NPCType<Content.Npcs.FlyingTerror.FlyingTerror>(), "Flying Terror")
				.SetWeight(VanillaWeights.QueenBee + .1f)   // Just after the Queen bee
				.SetSpawnInfoWithItem(ModContent.ItemType<Content.Items.Misc.HorridChunk>(), "at night")
				.SetDownedCallback(() => DownedSystem.downedFlyingTerror)
				.AddBoss(Mod, bossChecklistMod);

			// [Pre-Harmode mini-bosses]
			//
			new BossChecklistItemBuilder()
				.ForBoss(ModContent.NPCType<Content.Npcs.Bloodmoon.Bloodweaver>(), "Bloodweaver")
				.SetWeight(VanillaWeights.BloodMoon + .25f)   // Just after the Queen bee
				.SetSpawnInfo("Spawns during Bloodmoon")
				.SetDownedCallback(() => DownedSystem.downedFlyingTerror)
				.AddMiniBoss(Mod, bossChecklistMod);


			// [Harmode bosses]
			//
		}
	}

	/// <summary>
	/// Vanilla Boss/Event weights
	/// </summary>
	public static class VanillaWeights
	{
		// Bosses
		public const float KingSlime = 1f;
		public const float EyeOfCthulhu = 2f;
		public const float EaterOfWorlds = 3f;
		public const float QueenBee = 4f;
		public const float Skeletron = 5f;
		public const float DeerClops = 6f;
		public const float WallOfFlesh = 7f;
		public const float QueenSlime = 8f;
		public const float TheTwins = 9f;
		public const float TheDestroyer = 10f;
		public const float SkeletronPrime = 11f;
		public const float Plantera = 12f;
		public const float Golem = 13f;
		public const float DukeFishron = 14f;
		public const float EmpressOfLight = 15f;
		public const float Betsy = 16f;
		public const float LunaticCultist = 17f;
		public const float Moonlord = 18f;

		// Mini-bosses and Events
		public const float TorchGod = 1.5f;
		public const float BloodMoon = 2.5f;
		public const float GoblinArmy = 3.33f;
		public const float OldOnesArmy = 3.66f;
		public const float DarkMage = OldOnesArmy + 0.01f;
		public const float Ogre = SkeletronPrime + 0.01f; // Unlocked once a mechanical boss has been defeated
		public const float FrostLegion = 7.33f;
		public const float PirateInvasion = 7.66f;
		public const float PirateShip = PirateInvasion + 0.01f;
		public const float SolarEclipse = 11.5f;
		public const float PumpkinMoon = 13.25f;
		public const float MourningWood = PumpkinMoon + 0.01f;
		public const float Pumpking = PumpkinMoon + 0.02f;
		public const float FrostMoon = 13.5f;
		public const float Everscream = FrostMoon + 0.01f;
		public const float SantaNK1 = FrostMoon + 0.02f;
		public const float IceQueen = FrostMoon + 0.03f;
		public const float MartianMadness = 13.75f;
		public const float MartianSaucer = MartianMadness + 0.01f;
		public const float LunarEvent = LunaticCultist + 0.01f; // Happens immediately after the defeation of the Lunatic Cultist
	}

	public class BossChecklistItemBuilder
	{
		private string _name;
		public BossChecklistItemBuilder() { }

		private int _bossType;
		private string _bossName;
		private string _spawnInfo = null;
		private int _spawnInfoItem;
		private string _despawnInfo = null;
		private object _customPortraitCallback = null;
		private Func<bool> _downedCallback = () => false;
		private List<int> _collectibles;

		/// <summary>
		/// Gets the boss for this item and adds it's info to this item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public BossChecklistItemBuilder ForBoss(int type, string name)
		{
			_bossType = type;
			_bossName = name;
			return this;
		}
		public BossChecklistItemBuilder SetSpawnInfoWithItem(int itemType, string additionalInfo = "")
		{
			_spawnInfo = $"Use a [i:{itemType}] {additionalInfo}";
			_spawnInfoItem = itemType;
			return this;
		}
		public BossChecklistItemBuilder SetSpawnInfo(string info)
		{
			_spawnInfo = info;
			return this;
		}
		public BossChecklistItemBuilder SetDespawnInfo(string info)
		{
			_despawnInfo = info;
			return this;
		}
		public BossChecklistItemBuilder SetCustomPortrait(Action<SpriteBatch, Rectangle, Color> callback)
		{
			_customPortraitCallback = callback;
			return this;
		}
		public BossChecklistItemBuilder SetDownedCallback(Func<bool> callback)
		{
			_downedCallback = callback;
			return this;
		}
		public BossChecklistItemBuilder SetCollectibles(List<int> collectibleTypes)
		{
			_collectibles = collectibleTypes;
			return this;
		}
		private float _weight = 0;
		public BossChecklistItemBuilder SetWeight(float weight)
		{
			_weight = weight;
			return this;
		}

		public void AddBoss(Mod forMod, Mod bossChecklistMod)
		{
			bossChecklistMod.Call(
				"AddBoss",
				forMod,
				_bossName,
				_bossType,
				_weight,
				_downedCallback,
				true,               // / If the boss should show up on the checklist in the first place and when (here, always)
				_collectibles,
				_spawnInfoItem,
				_spawnInfo,
				_despawnInfo,
				_customPortraitCallback
			);
		}
		public void AddMiniBoss(Mod forMod, Mod bossChecklistMod)
		{
			bossChecklistMod.Call(
				"AddMiniBoss",
				forMod,
				_bossName,
				_bossType,
				_weight,
				_downedCallback,
				true,               // / If the boss should show up on the checklist in the first place and when (here, always)
				_collectibles,
				_spawnInfoItem,
				_spawnInfo,
				_despawnInfo,
				_customPortraitCallback
			);
		}
		// TODO:
		public void AddInvasion(Mod forMod, Mod bossChecklistMod)
		{
			bossChecklistMod.Call(
				"AddEvent",
				forMod,
				_bossName,
				_bossType,
				_weight,
				_downedCallback,
				true,               // / If the boss should show up on the checklist in the first place and when (here, always)
				_collectibles,
				_spawnInfoItem,
				_spawnInfo,
				_despawnInfo,
				_customPortraitCallback
			);
		}
	}
}
