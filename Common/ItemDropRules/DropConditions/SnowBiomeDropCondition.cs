using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace SupernovaMod.Common.ItemDropRules.DropConditions
{
	internal class SnowBiomeDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return Main.player[Player.FindClosest(info.npc.position, info.npc.width, info.npc.height)].ZoneSnow;
			}
			return false;
		}

		public bool CanShowItemDropInUI()
		{
			return false;
		}

		public string GetConditionDescription()
		{
			return "Drops in the Snow biome";
		}
	}
}
