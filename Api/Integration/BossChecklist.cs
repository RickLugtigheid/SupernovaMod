using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using System.Drawing;

namespace SupernovaMod.Api.Integration.BossChecklist
{
	/// <summary>
	/// A builder class for building a BossChecklist entry
	/// in an easy way.
	/// </summary>
	public class BossChecklistAdditionalEntryDataBuilder
	{
		private string _displayName;
		private LocalizedText _spawnInfo = null;
		private List<int> _spawnItems = null;
		private List<int> _collectibles = null;
		private Func<bool> _availability = null;
		private Func<NPC, LocalizedText> _despawnMessage = null;
		private Action<SpriteBatch, Rectangle, Color> _customPortrait = null;

		/// <summary>
		/// Sets the boss display name for this boss entry.
		/// </summary>
		/// <param name="displayName"></param>
		/// <returns></returns>
		public BossChecklistAdditionalEntryDataBuilder SetDisplayName(string displayName)
		{
			_displayName = displayName;
			return this;
		}
		/// <summary>
		/// Sets the spawn info for this boss entry.
		/// </summary>
		/// <param name="spawnInfo"></param>
		/// <returns></returns>
		public BossChecklistAdditionalEntryDataBuilder SetSpawnInfo(LocalizedText spawnInfo)
		{
			_spawnInfo = spawnInfo;
			return this;
		}
		public BossChecklistAdditionalEntryDataBuilder SetDespawnInfo(Func<NPC, LocalizedText> callback)
		{
			_despawnMessage = callback;
			return this;
		}
		public BossChecklistAdditionalEntryDataBuilder SetAvailability(Func<bool> callback)
		{
			_availability = callback;
			return this;
		}
		public BossChecklistAdditionalEntryDataBuilder SetDespawnMessage(Func<NPC, LocalizedText> callback)
		{
			_despawnMessage = callback;
			return this;
		}
		public BossChecklistAdditionalEntryDataBuilder SetCustomPortrait(Action<SpriteBatch, Rectangle, Color> callback)
		{
			_customPortrait = callback;
			return this;
		}

		public BossChecklistAdditionalEntryDataBuilder AddSpawnItem(int itemType)
		{
			if (_spawnItems == null)
			{
				_spawnItems = new List<int>();
			}
			_spawnItems.Add(itemType);
			return this;
		}
		public BossChecklistAdditionalEntryDataBuilder AddSpawnItem<T>() where T : ModItem => AddSpawnItem(ModContent.ItemType<T>());

		public BossChecklistAdditionalEntryDataBuilder AddCollectibleItem(int itemType)
		{
			if (_collectibles == null)
			{
				_collectibles = new List<int>();
			}
			_collectibles.Add(itemType);
			return this;
		}
		public BossChecklistAdditionalEntryDataBuilder AddCollectibleItem<T>() where T : ModItem => AddCollectibleItem(ModContent.ItemType<T>());

		public Dictionary<string, object> Build()
		{
			Dictionary<string, object> result = new Dictionary<string, object>();

			// Check if displayName is set
			//
			if (string.IsNullOrEmpty(_displayName))
			{
				result.Add("displayName", _displayName);
			}

			// Check if spawnInfo is set
			//
			if (_spawnInfo != null)
			{
				result.Add("spawnInfo", _spawnInfo);
			}

			// Check if spawnItems is set
			//
			if (_spawnItems != null)
			{
				result.Add("spawnItems", _spawnItems);
			}

			// Check if collectibles is set
			//
			if (_collectibles != null)
			{
				result.Add("collectibles", _collectibles);
			}

			// Check if availability is set
			//
			if (_availability != null)
			{
				result.Add("availability", _availability);
			}

			// Check if despawnMessage is set
			//
			if (_despawnMessage != null)
			{
				result.Add("despawnMessage", _despawnMessage);
			}

			// Check if customPortrait is set
			//
			if (_customPortrait != null)
			{
				result.Add("customPortrait", _customPortrait);
			}

			return result;
		}
	}

	public class BossChecklistItemBuilder
	{
		public BossChecklistItemBuilder() { }

		private int _bossType;
		private string _bossName;
		private Func<bool> _downedCallback = () => false;
		private BossChecklistAdditionalEntryDataBuilder _additionalEntryData;

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
		public BossChecklistItemBuilder ForBoss<T>() where T : ModNPC
		{
			_bossType = ModContent.NPCType<T>();
			_bossName = typeof(T).Name;
			return this;
		}
		public BossChecklistItemBuilder SetAdditionalEntryData(BossChecklistAdditionalEntryDataBuilder additionalEntryData)
		{
			_additionalEntryData = additionalEntryData;
			return this;
		}
		public BossChecklistItemBuilder SetDownedCallback(Func<bool> callback)
		{
			_downedCallback = callback;
			return this;
		}
		private float _weight = 0;
		public BossChecklistItemBuilder SetWeight(float weight)
		{
			_weight = weight;
			return this;
		}

		/// <summary>
		/// Adds a boss entry to the BossChecklist.
		/// </summary>
		/// <param name="forMod"></param>
		/// <param name="bossChecklistMod"></param>
		public void AddBoss(Mod forMod, Mod bossChecklistMod) => Add("LogBoss", forMod, bossChecklistMod);
		/// <summary>
		/// Adds a mini boss entry to the BossChecklist.
		/// </summary>
		/// <param name="forMod"></param>
		/// <param name="bossChecklistMod"></param>
		public void AddMiniBoss(Mod forMod, Mod bossChecklistMod) => Add("LogMiniBoss", forMod, bossChecklistMod);
		/// <summary>
		/// Adds a event entry to the BossChecklist.
		/// </summary>
		/// <param name="forMod"></param>
		/// <param name="bossChecklistMod"></param>
		public void AddEvent(Mod forMod, Mod bossChecklistMod) => Add("LogEvent", forMod, bossChecklistMod);

		/// <summary>
		/// Adds an entry to the BossChecklist.
		/// </summary>
		/// <param name="entryType"></param>
		/// <param name="forMod"></param>
		/// <param name="bossChecklistMod"></param>
		private void Add(string entryType, Mod forMod, Mod bossChecklistMod)
		{
			// https://github.com/JavidPack/BossChecklist/wiki/%5B1.4.4%5D-Boss-Log-Entry-Mod-Call
			List<object> arguments = new List<object>()
			{
				// [0] Entry Type
				entryType,
				// [1] Mod Instance
				forMod,
				// [2] Internal Name
				_bossName,
				// [3] Progression / Weight
				_weight,
				// [4] Downed Boolean callback
				_downedCallback,
				// [5] Boss ID / List of IDs
				_bossType,
				// [6] Additional Entry Data
				(_additionalEntryData?.Build()) ?? new Dictionary<string, object>()

			};
			bossChecklistMod.Call(arguments.ToArray());
		}
	}
}
