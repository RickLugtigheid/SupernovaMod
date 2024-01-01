using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.ID;
using SupernovaMod.Common.ItemDropRules.DropConditions;
using System;

namespace SupernovaMod.Common.GlobalNPCs
{
    public class DropRules : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			// Drop from any Skeleton
			//
			if (NPCID.Sets.Skeletons[npc.type])
			{
				// 1/7 (14.28571%) Drop chance after the EoC is downed
				//
				npcLoot.Add(GetDropRule<EoCDownedDropCondition>(conditionalRule =>
				{
					conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Materials.BoneFragment>(), 7, maximumDropped: 3));
				}));
			}

			// Drop from any demon
			//
			if (npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon || npc.type == NPCID.RedDevil)
			{
				// 1/30 (3.3333%) Drop chance
				//
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.Accessories.DemonHorns>(), 30));
			}

			// Drom from red devils
			//
			if (npc.type == NPCID.RedDevil)
			{
				// 1/30 (3.3333%) Drop chance
				//
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.Accessories.SealOfSamael>(), 30));
			}

			// Drop from pre-hardmode ice enemies
			//
			if (npc.type == NPCID.IceSlime || npc.type == NPCID.SpikedIceSlime || npc.type == NPCID.IceBat)
			{
				// 1/10 (10%) Drop chance when the Flying Terror is downed
				//
				npcLoot.Add(GetDropRule<FlyingTerrorDownedDropCondition>(conditionalRule =>
				{
					conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Materials.Rime>(), 10, maximumDropped: 2));
				}));
			}

			// Drom from Wolfs
			//
			if (npc.type == NPCID.Wolf)
			{
				// 1/30 (3.3333%) Drop chance
				//
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.Weapons.Melee.BattleAxe>(), 30));
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

		public static IItemDropRule GetDropRule<T>(Action<IItemDropRule> conditionCallback) where T : IItemDropRuleCondition, new()
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
			conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Weapons.Magic.StaffOfThorns>(), 80), true);
		}
		public void NPCBiomeGlowingMushroomLoot(IItemDropRule conditionalRule)
		{
			// 1/50 (2%) Drop chance
			//
			conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Accessories.BagOfFungus>(), 50), true);
		}
		public void NPCBiomeSnowLoot(IItemDropRule conditionalRule)
		{
		}
		public void NPCBiomeEvilLoot(IItemDropRule conditionalRule)
		{
			// 1/120 (0.83333%) Drop chance
			//
			conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Accessories.SacrificialTalisman>(), 120), true);
		}
		#endregion

		#region Event Loot Handlers
		public void NPCEventBloodMoonLoot(IItemDropRule conditionalRule)
		{
			// 1/10 (10%) drop chance
			//
			conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Materials.BloodShards>(), 10, maximumDropped: 3), true);
		}
		#endregion
	}
}
