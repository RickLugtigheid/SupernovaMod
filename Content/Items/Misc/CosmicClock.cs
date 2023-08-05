using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Misc
{
    public class CosmicClock : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cosmic Clock");
            // Tooltip.SetDefault("A item that can change the time");
        }

        public override void SetDefaults()
        {
            Item.useTurn = true;
            Item.width = 26;
            Item.height = 18;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 70;
            //Item.UseSound = new LegacySoundStyle(SoundID.MoonLord, 0);
            Item.UseSound = SoundID.MoonLord;
            Item.useAnimation = 60;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(1, 0, 0);
            Item.maxStack = 1;
        }
		public override bool CanUseItem(Player player)
		{
            if (Main.dayTime == true)
			{
                Main.time = 54000;
            }
            else
			{
                Main.time = 32400;
            }
            return true;
        }
    }
}