using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class MarbleMagnum : SupernovaGunItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Marble Magnum");
            // Tooltip.SetDefault("Fires a dense spread of bullets");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 6;
            Item.width = 40;
            Item.crit = 3;
            Item.height = 20;
            Item.useAnimation = 64;
            Item.useTime = 64;
            Item.noMelee = true; //so the item's animation doesn't do damage
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 6, 23);
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item38;
            Item.shootSpeed = 8f;
            Item.useAmmo = AmmoID.Bullet;

            Item.scale = .8f;

            Gun.spread = .5f;
            Gun.recoil = 1;

            Gun.style = GunStyle.Shotgun;
            Gun.shotgunMinShots = 3;
            Gun.shotgunMaxShots = 6;
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
            recipe.AddIngredient(ItemID.MarbleBlock, 20);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 7);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.FirearmManual>(), 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}