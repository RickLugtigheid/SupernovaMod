using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Tools
{
    public class VerglasHamaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Hamaxe");
        }

        public override void SetDefaults()
        {
            
            item.damage = 18;
            item.width = 24;
            item.height = 24;
            
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.knockBack = 7;
            item.value = Item.buyPrice(0, 3, 54);
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;

            item.hammer = 65;
            item.axe = 135;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("VerglasBar"), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
