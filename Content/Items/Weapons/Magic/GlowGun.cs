using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
    public class GlowGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Glow Gun");
        }
        public override Vector2? HoldoutOffset() => new Vector2(-4, -2);

        public override void SetDefaults()
        {
            //base.SetDefaults();

            Item.damage = 12;
            Item.width = 50;
            Item.height = 24;
            Item.crit = 3;
            Item.useTime = 48;
            Item.useAnimation = 48;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 3, 50, 0);
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item38;
            Item.shoot = ModContent.ProjectileType<Projectiles.Magic.GlowGunProj>();
            Item.shootSpeed = 5;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.scale = .75f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.FirearmManual>(), 2);
            recipe.AddIngredient(ItemID.GlowingMushroom, 20);
            recipe.AddIngredient(ItemID.StickyGlowstick, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}