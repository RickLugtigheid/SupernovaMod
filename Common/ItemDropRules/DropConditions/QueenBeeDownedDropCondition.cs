using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace Supernova.Common.ItemDropRules.DropConditions
{
	internal class QueenBeeDownedDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return NPC.downedQueenBee;
			}
			return false;
		}

		public bool CanShowItemDropInUI()
		{
			return true;
		}

		public string GetConditionDescription()
		{
			return "Drops after the Queen Bee is defeated";
		}
	}
}
