using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Ammo
{
    public class WoodenBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;

            // DisplayName.SetDefault("Wooden Bullet");
        }

        public override void SetDefaults()
        {
            Item.damage = 4;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.value = 10;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.Bullets.WoodenBullet>();
            Item.shootSpeed = 1f;
            Item.ammo = AmmoID.Bullet; // To assign the ammo type.

            Item.DamageType = DamageClass.Ranged;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ItemID.Wood, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
