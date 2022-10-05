using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Bosses.FlyingTerror
{
    public class FlyingTerrorBag : ModItem
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

		public override void OpenBossBag(Player player)
        {
            int rand = Main.rand.Next(2, 3);

            for (int i = 0; i < rand; i++)
            {
                int item = Main.rand.Next(2);

                switch (item)
                {
                    case 0:
                        player.QuickSpawnItem(Item.GetSource_Loot(), ModContent.ItemType<TerrorInABottle>());
                        break;
                    case 1:
                        player.QuickSpawnItem(Item.GetSource_Loot(), ModContent.ItemType<TerrorCleaver>());
                        break;
                    case 2:
                        player.QuickSpawnItem(Item.GetSource_Loot(), ModContent.ItemType<TerrorRecurve>());
                        break;
                    case 3:
                        player.QuickSpawnItem(Item.GetSource_Loot(), ModContent.ItemType<TerrorTome>());
                        break;
                    case 4:
                        player.QuickSpawnItem(Item.GetSource_Loot(), ModContent.ItemType<BlunderBuss>());
                        break;
                }
            }
        }
    }
}
