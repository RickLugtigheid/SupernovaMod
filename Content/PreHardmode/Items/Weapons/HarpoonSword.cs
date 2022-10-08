using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class HarpoonSword : ModItem
	{
		public override void SetStaticDefaults()
		{
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Harpoon Blade");
            Tooltip.SetDefault("Right click to shoot a harpoon at your enemies");
            // Tooltip2.SetDefault("On Hit it lets the enemey bleed.");
            Item.staff[Item.type] = true;
        }

        private int _swordDamage = 24;
        public override void SetDefaults()
		{
            Item.damage = _swordDamage;
            Item.crit = 4;
            Item.width = 40;
			Item.height = 40;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = 1;
			Item.value = 10000;
			Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;
            SetDefaultsSword();

            Item.DamageType = DamageClass.Melee;
        }

        private void SetDefaultsSword()
		{
            // Left Click has no projectile
            Item.shootSpeed = 0f;
            Item.shoot = ProjectileID.None;
            Item.UseSound = SoundID.Item1;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7;
            Item.damage = _swordDamage;
        }
        private void SetDefaultsHarpoon()
        {
            Item.shoot = ProjectileID.Harpoon;
            Item.damage = 28;
            Item.shootSpeed = 30f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;

            Item.UseSound = SoundID.Item10;
        }

        public override bool AltFunctionUse(Player player) => true;

		public override void UseAnimation(Player player)
		{
            if (player.altFunctionUse == ItemAlternativeFunctionID.ActivatedAndUsed)
            {
                SetDefaultsHarpoon();
            }
            else
            {
                SetDefaultsSword();
            }
            base.UseAnimation(player);
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
