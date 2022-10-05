using Terraria;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Bosses.HarbingerOfAnnihilation
{
    public class HarbingersCrest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harbingers Crest");
            Tooltip.SetDefault("You have a 1 in 3 change on to take fall damage" +
                "\nAnd increaces Mining speed");
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
            if (Main.rand.Next(2) == 1)
                player.noFallDmg = true;
            player.pickSpeed -= 0.38f;
        }
    }
}
