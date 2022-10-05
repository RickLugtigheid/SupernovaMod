using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class HarpoonSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpoon Blade");
            Tooltip.SetDefault("Right click to shoot a harpoon at your enemies");
            // Tooltip2.SetDefault("On Hit it lets the enemey bleed.");
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
		{
            Item.damage = 24;
            Item.crit = 4;
            Item.width = 40;
			Item.height = 40;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = 1;
			Item.value = 10000;
			Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) // Right Click function
            {
                Item.shoot = ProjectileID.Harpoon;
                Item.damage = 28;
                Item.shootSpeed = 30f;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.knockBack = 1;

                Item.UseSound = SoundID.Item10;
            }
            else // Default Left Click
            {
                // Left Click has no projectile
                Item.shootSpeed = 0f;
                Item.shoot = ProjectileID.None;
                Item.UseSound = SoundID.Item1;
				
				Item.useStyle = ItemUseStyleID.Swing;
                Item.knockBack = 7;
                Item.damage = 31;
            }
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronBar, 7);
            //recipe.anyIronBar = true;
            recipe.acceptedGroups.AddRange(new[] { RecipeGroupID.IronBar });
            recipe.AddIngredient(ItemID.Harpoon);
            recipe.AddIngredient(ItemID.SharkFin);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
