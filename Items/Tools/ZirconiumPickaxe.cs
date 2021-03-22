using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Tools
{
    public class ZirconiumPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zirconium Pickaxe");
        }

        public override void SetDefaults()
        {
            
            item.damage = 6; // Base Damage of the Weapon
            item.width = 24; // Hitbox Width
            item.height = 24; // Hitbox Height
            
            item.useTime = 15; // Speed before reuse
            item.useAnimation = 15; // Animation Speed
            item.useStyle = 1; // 1 = Broadsword 
            item.knockBack = 1.5f; // Weapon Knockbase: Higher means greater "launch" distance
            item.value = 25500; // 10 | 00 | 00 | 00 : Platinum | Gold | Silver | Bronze
            item.rare = Rarity.Green; // Item Tier
            item.UseSound = SoundID.Item1; // Sound effect of item on use 
            item.autoReuse = true; // Do you want to torture people with clicking? Set to false

            item.pick = 50; // Pick Power - Higher Value = Better
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 3);
            recipe.AddIngredient(mod.GetItem("ZirconiumBar"), 12);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
