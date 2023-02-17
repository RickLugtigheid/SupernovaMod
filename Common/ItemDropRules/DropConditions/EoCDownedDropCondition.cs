using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace SupernovaMod.Common.ItemDropRules.DropConditions
{
	internal class EoCDownedDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return NPC.downedBoss1;
			}
			return false;
		}

		public bool CanShowItemDropInUI()
		{
			return true;
		}

		public string GetConditionDescription()
		{
			return "Drops after the Eye of Cthulhu is defeated";
		}
	}
}
