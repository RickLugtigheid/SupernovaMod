using System.Collections.Generic;
using Terraria.GameContent.ItemDropRules;

namespace SupernovaMod.Api.ChestLoot
{
	public class SupernovaItemDropDatabase : ItemDropDatabase
	{
		private Dictionary<int, List<IItemDropRule>> _entriesByChestId = new Dictionary<int, List<IItemDropRule>>();
		private Dictionary<ChestFrameType, List<int>> _chestIdsByType = new Dictionary<ChestFrameType, List<int>>();

		public List<IItemDropRule> GetRulesForChestID(int chestID)
		{
			List<IItemDropRule> list = new List<IItemDropRule>();
			if (_entriesByChestId.TryGetValue(chestID, out var value))
			{
				list.AddRange(value);
			}

			return list;
		}

		public IItemDropRule RegisterToChest(ChestFrameType type, IItemDropRule entry)
		{
			RegisterToChestID((int)type, entry);
			if (type > 0 && _chestIdsByType.TryGetValue(type, out var value))
			{
				for (int i = 0; i < value.Count; i++)
				{
					RegisterToChestID(value[i], entry);
				}
			}

			return entry;
		}

		public IItemDropRule RegisterToMultipleChests(IItemDropRule entry, params ChestFrameType[] chestIDs)
		{
			for (int i = 0; i < chestIDs.Length; i++)
			{
				RegisterToChest(chestIDs[i], entry);
			}

			return entry;
		}

		public void RegisterToChestID(int chestID, IItemDropRule entry)
		{
			if (!_entriesByChestId.ContainsKey(chestID))
			{
				_entriesByChestId[chestID] = new List<IItemDropRule>();
			}

			_entriesByChestId[chestID].Add(entry);
		}

		private void RemoveFromChestID(int chestID, IItemDropRule entry)
		{
			if (_entriesByChestId.ContainsKey(chestID))
			{
				_entriesByChestId[chestID].Remove(entry);
			}
		}

		public IItemDropRule RemoveFromChest(ChestFrameType type, IItemDropRule entry)
		{
			RemoveFromChestID((int)type, entry);
			if (type > 0 && _chestIdsByType.TryGetValue(type, out var value))
			{
				for (int i = 0; i < value.Count; i++)
				{
					RemoveFromChestID(value[i],entry);
				}
			}

			return entry;
		}
	}
}
