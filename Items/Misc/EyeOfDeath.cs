using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Misc
{
    public class EyeOfDeath : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye Of Death");
            Tooltip.SetDefault("Stare into the Eye of Death and go back to where you died");
        }

        public override void SetDefaults()
        {
            item.useTurn = true;
            item.width = 26;
            item.height = 18;
            item.useStyle = 4;
            item.useTime = 90;
            item.UseSound = new LegacySoundStyle(SoundID.MoonLord, 0);
            item.useAnimation = 90;
            item.rare = Rarity.LigtRed;
            item.value = Item.sellPrice(1, 0, 0);
            item.maxStack = 1;
        }

        public override bool CanUseItem(Player player) => player.showLastDeath;

        public override bool UseItem(Player player)
        {
            if (Main.rand.Next(2) == 0)
                Dust.NewDust(player.position, player.width, player.height, 15, 0.0f, 0.0f, 150, Color.Red, 1.1f);
            if (player.itemAnimation == item.useAnimation / 2)
            {
                for (int index = 0; index < 70; ++index)
                    Dust.NewDust(player.position, player.width, player.height, 15, (float)(player.velocity.X * 0.5), (float)(player.velocity.Y * 0.5), 150, Color.Red, 1.5f);
                player.Teleport(player.lastDeathPostion, -69);
                player.Center = player.lastDeathPostion;
                if (Main.netMode == NetmodeID.SinglePlayer)
                    NetMessage.SendData(65, -1, -1, null, 0, (float)player.whoAmI, player.lastDeathPostion.X, player.lastDeathPostion.Y, 3);

                for (int index = 0; index < 70; ++index)
                    Dust.NewDust(player.position, player.width, player.height, 15, 0.0f, 0.0f, 150, Color.Red, 1.5f);
                return true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Bone, 46);
            recipe.AddIngredient(ItemID.SoulofNight, 23);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}