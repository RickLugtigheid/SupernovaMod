using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Misc
{
    public class EyeOfDeath : ModItem
    {
        public override void SetDefaults()
        {
            Item.useTurn = true;
            Item.width = 26;
            Item.height = 18;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 90;
            Item.UseSound = SoundID.MoonLord;
            Item.useAnimation = 90;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(1, 0, 0);
            Item.maxStack = 1;
        }
		public override bool? UseItem(Player player)
		{
            if (player.whoAmI == Main.myPlayer)
			{
                if (Main.rand.NextBool(2))
                {
                    Dust.NewDust(player.position, player.width, player.height, DustID.Corruption, 0.0f, 0.0f, 150, Color.Red, 1.1f);
                }

                if (player.itemAnimation == Item.useAnimation / 2)
                {
                    for (int index = 0; index < 70; ++index)
                    {
                        Dust.NewDust(player.position, player.width, player.height, DustID.Corruption, (float)(player.velocity.X * 0.5), (float)(player.velocity.Y * 0.5), 150, Color.Red, 1.5f);
                    }

                    player.Teleport(player.lastDeathPostion, -69);
                    player.Center = player.lastDeathPostion;

                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, (float)player.whoAmI, player.lastDeathPostion.X, player.lastDeathPostion.Y, 3);
                    }

                    for (int index = 0; index < 70; ++index)
                    {
                        Dust.NewDust(player.position, player.width, player.height, DustID.Corruption, 0.0f, 0.0f, 150, Color.Red, 1.5f);
                    }
                    return true;
                }
            }
            return false;
		}
        public override bool CanUseItem(Player player) => player.showLastDeath;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Bone, 15);
            recipe.AddIngredient(ItemID.SoulofNight, 8);
            recipe.AddIngredient(ItemID.CrystalShard, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}