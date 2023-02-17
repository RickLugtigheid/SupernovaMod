using Supernova.Content.Items.BaseItems;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.Npcs.Bosses.FlyingTerror
{
    public class FlyingTerrorBag : SupernovaBossBag
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Expert;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true; // This bag is opened with right click
        }

        public override void OnConsumeItem(Player player)
        {
            base.OnConsumeItem(player);
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<FlyingTerror>()));
            itemLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<TerrorInABottle>()));
            itemLoot.Add(ItemDropRule.OneFromOptions(1, new int[]
            {
                ModContent.ItemType<TerrorCleaver>(),
                ModContent.ItemType<TerrorRecurve>(),
                ModContent.ItemType<TerrorTome>(),
                ModContent.ItemType<BlunderBuss>()
            }));
        }
    }
}
