using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace Supernova.Api.ChestLoot
{
	public class ChestLoot
	{
		private Dictionary<ChestFrameType, List<IItemDropRule>> _ruleMap = new Dictionary<ChestFrameType, List<IItemDropRule>>();
		public void Add(ChestFrameType chestType, IItemDropRule rule)
		{
			if (!_ruleMap.ContainsKey(chestType))
			{
				_ruleMap.Add(chestType, new List<IItemDropRule>());
			}

			_ruleMap[chestType].Add(rule);
		}

		public bool TryGetloot(ChestFrameType chestType, out int itemId, out ChestLootInjectRule lootInjectRule)
		{
			itemId = -1;
			lootInjectRule = ChestLootInjectRule.AddItem;

			if (!_ruleMap.ContainsKey(chestType))
			{
				return false;
			}

			DropAttemptInfo dropInfo = new DropAttemptInfo()
			{
				IsExpertMode = Main.expertMode,
				IsMasterMode = Main.masterMode,
				rng = Main.rand
			};
			foreach (IItemDropRule rule in _ruleMap[chestType])
			{
				return TryGetRuleItem(rule, out itemId, out lootInjectRule);
			}
			return false;
		}

		private bool TryGetRuleItem(IItemDropRule rule, out int itemId, out ChestLootInjectRule injectRule)
		{
			itemId = -1;
			injectRule = ChestLootInjectRule.AddItem;
			Type type = rule.GetType();
			if (type == typeof(ChestLootRule))
			{
				ChestLootRule lootRule = (ChestLootRule)rule;

				itemId = lootRule.itemId;
				injectRule = lootRule.injectRule;
				return Main.rand.NextBool(lootRule.chanceDenominator);
			}
			return false;
		}
	}
	public struct ChestLootRule : IItemDropRule
	{
		public int itemId;
		public int chanceDenominator;
		public ChestLootInjectRule injectRule;
		public ChestLootRule(int itemType, int chanceDenominator = 1, ChestLootInjectRule injectRule = ChestLootInjectRule.AddItem)
		{
			this.itemId = itemType;
			this.chanceDenominator = chanceDenominator;
			this.injectRule = injectRule;
		}

		public List<IItemDropRuleChainAttempt> ChainedRules => throw new NotImplementedException();

		public bool CanDrop(DropAttemptInfo info)
		{
			throw new NotImplementedException();
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			throw new NotImplementedException();
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			throw new NotImplementedException();
		}
	}
}
