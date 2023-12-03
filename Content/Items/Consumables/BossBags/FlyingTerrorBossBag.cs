using SupernovaMod.Content.Items.BaseItems;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Consumables.BossBags
{
    public class FlyingTerrorBossBag : SupernovaBossBag
    {
        public override void SetStaticDefaults()
        {
			// This set is one that every boss bag should have.
			// It will create a glowing effect around the item when dropped in the world.
			// It will also let our boss bag drop dev armor..
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; // ..But this set ensures that dev armor will only be dropped on special world seeds, since that's the behavior of pre-hardmode boss bags.

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Expert;
			Item.expert = true; // This makes sure that "Expert" displays in the tooltip and the item name color changes
        }

		public override bool CanRightClick() => true; // This bag is opened with right click

		public override void ModifyItemLoot(ItemLoot itemLoot)
        {
			// TODO: itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<FlyingTerrorMask>(), 7));
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<Npcs.FlyingTerror.FlyingTerror>()));
            //itemLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<TerrorInABottle>()));
            itemLoot.Add(ItemDropRule.OneFromOptions(1, new int[]
            {
                ModContent.ItemType<Weapons.Melee.TerrorCleaver>(),
                ModContent.ItemType<Weapons.Ranged.TerrorRecurve>(),
                ModContent.ItemType<Weapons.Magic.TerrorTome>(),
               // ModContent.ItemType<Items.Weapons.Melee.BlunderBuss>()
            }));
		}
    }
}
