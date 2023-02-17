using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace SupernovaMod.Common.ItemDropRules.DropConditions
{
	internal class ExpertModDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return Main.expertMode;
			}
			return false;
		}

		public bool CanShowItemDropInUI()
		{
			return false;
		}

		public string GetConditionDescription()
		{
			return "Drops in expert mode";
		}
	}
}
