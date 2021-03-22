using Terraria;
using Terraria.ModLoader;

namespace Supernova.Npcs.Bosses.FlyingTerror
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
            item.maxStack = 999;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.rare = Rarity.Rainbow;
            item.expert = true;
        }
        public override int BossBagNPC => mod.NPCType("FlyingTerror");

        public override bool CanRightClick()
        {
            return true; // This bag is opened with right click
        }

        public override void OpenBossBag(Player player)
        {
            int rand = Main.rand.Next(2, 3);

            for (int i = 0; i < rand; i++)
            {
                int item = Main.rand.Next(4);

                switch (item)
                {
                    case 0:
                        player.QuickSpawnItem(mod.ItemType("TerrorInABottle"));
                        break;
                    case 1:
                        player.QuickSpawnItem(mod.ItemType("TerrorCleaver"));
                        break;
                    case 2:
                        player.QuickSpawnItem(mod.ItemType("TerrorRecurve"));
                        break;
                    case 3:
                        player.QuickSpawnItem(mod.ItemType("TerrorTome"));
                        break;
                    case 4:
                        player.QuickSpawnItem(mod.ItemType("BlunderBuss"));
                        break;
                }
            }
        }
    }
}
