using Microsoft.Xna.Framework;
using Supernova.Content.PreHardmode.Items.Weapons.BaseWeapons;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons.Magic
{
    public class GlowGun : SupernovaGunItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Glow Gun");
        }
        public override Vector2? HoldoutOffset() => new Vector2(-4, -2);

        public override void SetDefaults()
        {
            //base.SetDefaults();

            Item.damage = 21;
            Item.width = 40;
            Item.crit = 4;
            Item.height = 20;
            Item.useTime = 42;
            Item.useAnimation = 42;
            Item.knockBack = 1.2f;
            Item.value = Item.buyPrice(0, 2, 50, 0);
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item38;
            Item.shoot = ModContent.ProjectileType<Global.Projectiles.Magic.GlowGunProj>();
            Item.shootSpeed = 3;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
			Item.useStyle = ItemUseStyleID.Shoot;

			Item.scale = .8f;

            Gun.recoil = 1.6f;
            Gun.spread = 3;
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