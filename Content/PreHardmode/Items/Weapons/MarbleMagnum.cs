using Microsoft.Xna.Framework;
using Supernova.Api;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
	public class MarbleMagnum : ModShotgun
    {
        public override float SpreadAngle => 2;
		public override int MinShots => 3;
		public override int MaxShots => 6;

		public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Marble Magnum");
            Tooltip.SetDefault("Fires a dense spread of bullets");
        }

        public override Vector2? HoldoutOffset() => new Vector2(-2, 0);


        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.width = 40;
            Item.crit = 3;
            Item.height = 20;
            Item.useAnimation = 68;
            Item.useTime = 68;
            Item.useStyle = 5;
            Item.noMelee = true; //so the item's animation doesn't do damage
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 6, 23);
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item38;
            Item.shoot = 10; //idk why but all the guns in the vanilla source have this
            Item.shootSpeed = 8f;
            Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged; // For Ranged Weapon

            Item.scale = .8f;
        }

		/*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            Vector2[] speeds = Calc.RandomSpread(velocity.X, velocity.Y, 8, 0.0025f, 6);
            for (int i = 0; i < Main.rand.Next(3, 6); ++i)
            {
                Projectile.NewProjectile(source, position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockback, player.whoAmI);
            }
            return false;
        }*/
		/*public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2[] speeds = Calc.RandomSpread(speedX, speedY, 8, 0.0025f, 6);
            for (int i = 0; i < Main.rand.Next(3, 6); ++i)
            {
                Projectile.NewProjectile(position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }*/

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup(RecipeGroupID.Sand, 12);
            recipe.AddIngredient(ItemID.MarbleBlock, 37);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 7);
            recipe.AddIngredient(ModContent.ItemType<Materials.FirearmManual>(), 2);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }
}