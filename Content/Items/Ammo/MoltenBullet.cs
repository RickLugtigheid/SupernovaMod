using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Ammo
{
    public class MoltenBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Molten Bullet");
        }

        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 0f;
            Item.value = 10;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.Bullets.MoltenBullet>();
            Item.shootSpeed = 4f;
            Item.ammo = AmmoID.Bullet;

            Item.DamageType = DamageClass.Ranged;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(25);
            recipe.AddIngredient(ItemID.HellstoneBar);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
