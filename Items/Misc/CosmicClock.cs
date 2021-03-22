using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Misc
{
    public class CosmicClock : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Clock");
            Tooltip.SetDefault("A item that can change the time");
        }

        public override void SetDefaults()
        {
            item.useTurn = true;
            item.width = 26;
            item.height = 18;
            item.useStyle = 4;
            item.useTime = 70;
            item.UseSound = new LegacySoundStyle(SoundID.MoonLord, 0);
            item.useAnimation = 60;
            item.rare = 4;
            item.value = Item.sellPrice(1, 0, 0);
            item.maxStack = 1;
        }

        public override bool UseItem(Player player)
        {
            if ( Main.dayTime == true)
                Main.time = 54000;
            else
                Main.time = 32400;
            return true;
        }
    }
}