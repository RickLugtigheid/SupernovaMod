using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Tools
{
    public class VerglasPick : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Pick");
        }

        public override void SetDefaults()
        {
            
            item.damage = 12;
            item.width = 24;
            item.height = 24;
            
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = 1;
            item.knockBack = 7;
            item.value = Item.buyPrice(0, 3, 54);
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;

            item.pick = 100;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("VerglasBar"), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
