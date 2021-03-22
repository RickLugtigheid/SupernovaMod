using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Tools
{
    public class ZirconiumAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zirconium Axe");
        }

        public override void SetDefaults()
        {
            
            item.damage = 6; // Base Damage of the Weapon
            item.width = 24; // Hitbox Width
            item.height = 24; // Hitbox Height
            
            item.useTime = 21; // Speed before reuse
            item.useAnimation = 21; // Animation Speed
            item.useStyle = 1; // 1 = Broadsword 
            item.knockBack = 1f; // Weapon Knockbase: Higher means greater "launch" distance
            item.value = 25500; // 10 | 00 | 00 | 00 : Platinum | Gold | Silver | Bronze
            item.rare = Rarity.Green; // Item Tier
            item.UseSound = SoundID.Item1; // Sound effect of item on use 
            item.autoReuse = true; // Do you want to torture people with clicking? Set to false

            item.axe = 50; // Axe Power - Higher Value = Better
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 3);
            recipe.AddIngredient(mod.GetItem("ZirconiumBar"), 9);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
