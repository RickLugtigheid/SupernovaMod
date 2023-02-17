using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace Supernova.Content.Npcs.Bosses.HarbingerOfAnnihilation
{
    public class HarbingersCrest : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Harbingers Crest");
            Tooltip.SetDefault("Has a 1 in 3 chance to not take fall damage" +
                "\nAnd increased Mining speed");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.rare = -12;
            Item.expert = true;
            Item.value = Item.buyPrice(0, 4, 80, 0);
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            if (Main.rand.NextBool(2))
            {
                player.noFallDmg = true;
            }
            player.pickSpeed -= 0.38f;
        }
    }
}
