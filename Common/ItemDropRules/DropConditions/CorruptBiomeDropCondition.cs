﻿using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace Supernova.Common.ItemDropRules.DropConditions
{
	internal class CorruptBiomeDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return Main.player[Player.FindClosest(info.npc.position, info.npc.width, info.npc.height)].ZoneCorrupt;
			}
			return false;
		}

		public bool CanShowItemDropInUI()
		{
			return true;
		}

		public string GetConditionDescription()
		{
			return "Drops in the Corrupt biome";
		}
	}
}
