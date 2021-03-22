using Terraria;
using Terraria.ModLoader;

namespace Supernova.Npcs.Bosses.HarbingerOfAnnihilation
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
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.rare = -12;
            item.expert = true;
            item.value = Item.buyPrice(0, 4, 80, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            if (Main.rand.Next(2) == 1)
                player.noFallDmg = true;
            player.pickSpeed -= 0.38f;
        }
    }
}
