using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace SupernovaMod.Common.ItemDropRules.DropConditions
{
	internal class ExpertModeDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return info.IsExpertMode;
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
	internal class NormalModeDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return !info.IsExpertMode;
			}
			return false;
		}

		public bool CanShowItemDropInUI()
		{
			return false;
		}

		public string GetConditionDescription()
		{
			return "Drops in normal mode";
		}
	}
}
