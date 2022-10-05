using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace Supernova.Common.ItemDropRules.DropConditions
{
	internal class BloodMoonDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return Main.bloodMoon;
			}
			return false;
		}

		public bool CanShowItemDropInUI()
		{
			return true;
		}

		public string GetConditionDescription()
		{
			return "Drops during Bloodmoon";
		}
	}
}
