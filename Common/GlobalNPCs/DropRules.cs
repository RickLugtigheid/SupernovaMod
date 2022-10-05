using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.ID;
using Supernova.Common.ItemDropRules.DropConditions;
using System;

namespace Supernova.Common.GlobalNPCs
{
	public class DropRules : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			// Drop from any zombie
			//
			if (NPCID.Sets.Zombies[npc.type])
			{
				// 1/7 (14,28571%) Srop chance after the EoC is downed
				//
				if (NPC.downedBoss1)
				{
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.PreHardmode.Items.Materials.BoneFragment>(), 3, maximumDropped: 3));
				}
			}

			// Drop from any demon
			if (npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon || npc.type == NPCID.RedDevil)
			{
				// 1/30 (3.3333%) Drop chance
				//
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.PreHardmode.Items.Accessories.DemonHorns>(), 30));
			}

			/* Register Event Handlers */
			npcLoot.Add(GetDropRule<BloodMoonDropCondition>(NPCEventBloodMoonLoot));

			/* Register Biome Handlers */
			npcLoot.Add(GetDropRule<MushroomBiomeDropCondition>(NPCBiomeGlowingMushroomLoot));
			npcLoot.Add(GetDropRule<JungleBiomeDropCondition>(NPCBiomeJungleLoot));
			npcLoot.Add(GetDropRule<SnowBiomeDropCondition>(NPCBiomeSnowLoot));
			npcLoot.Add(GetDropRule<CorruptBiomeDropCondition>(NPCBiomeEvilLoot));
			npcLoot.Add(GetDropRule<CrimsonBiomeDropCondition>(NPCBiomeEvilLoot));
			npcLoot.Add(GetDropRule<CrimsonBiomeDropCondition>(NPCBiomeEvilLoot));

			base.ModifyNPCLoot(npc, npcLoot);
		}

		private IItemDropRule GetDropRule<T>(Action<IItemDropRule> conditionCallback) where T : IItemDropRuleCondition, new()
		{
			T dropCondition = new T();
			IItemDropRule conditionalRule = new LeadingConditionRule(dropCondition);
			conditionCallback(conditionalRule);
			return conditionalRule;
		}

		#region Biome Loot Handlers
		public void NPCBiomeJungleLoot(IItemDropRule conditionalRule)
		{
			// 1/80 (1.25%) Drop chance
			//
			conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.PreHardmode.Items.Weapons.StaffOfThorns>(), 80));
		}
		public void NPCBiomeGlowingMushroomLoot(IItemDropRule conditionalRule)
		{
			// 1/50 (2%) Drop chance
			//
			conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.PreHardmode.Items.Accessories.BagOfFungus>(), 50));
		}
		public void NPCBiomeSnowLoot(IItemDropRule conditionalRule)
		{
			// 1/5 (20%) drop chance after the Queen bee is downed
			//
			if (NPC.downedQueenBee == true)
			{
				conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.PreHardmode.Items.Materials.Rime>(), 5, maximumDropped: 2));
			}
		}
		public void NPCBiomeEvilLoot(IItemDropRule conditionalRule)
		{
			// 1/120 (0.83333%) Drop chance
			//
			conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.PreHardmode.Items.Accessories.SacrificialTalisman>(), 120));
		}
		#endregion

		#region Event Loot Handlers
		public void NPCEventBloodMoonLoot(IItemDropRule conditionalRule)
		{
			// 1/4 (25%) drop chance
			//
			conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.PreHardmode.Items.Materials.BloodShards>(), 4, maximumDropped: 3));
		}
		#endregion
	}
}
