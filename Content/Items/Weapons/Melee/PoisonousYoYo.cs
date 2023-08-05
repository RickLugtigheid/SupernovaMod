using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class PoisonousYoYo : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Poisonous YoYo");
        }

        public override void SetDefaults()
        {
            Item.knockBack = 1.8f;
            Item.damage = 24;
            Item.crit = 4;
            Item.useStyle = ItemUseStyleID.Shoot; // The style used for YoYos.
            Item.width = 24;
            Item.height = 24;
            Item.noUseGraphic = true; // Doesn't show Item in Hand.
            Item.noMelee = true; // Don't damage enemies with the hand hitbox.
            Item.channel = true; // ???
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 25;
            Item.useTime = 56;
            Item.shoot = ModContent.ProjectileType<Projectiles.Melee.Yoyos.PoisonousYoYo>();
            Item.shootSpeed = 23f; // How fast the projectile is shot.
            Item.value = Item.buyPrice(0, 4, 43, 70); // Another way to handle value of item.
            Item.rare = ItemRarityID.Orange;

            Item.DamageType = DamageClass.Throwing;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.JungleYoyo);
            recipe.AddIngredient(ItemID.Stinger, 7);
            recipe.AddIngredient(ItemID.JungleSpores, 3);
            recipe.AddIngredient(ItemID.Vine, 4);
            recipe.AddIngredient(ItemID.BeeWax);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
