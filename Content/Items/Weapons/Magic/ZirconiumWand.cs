using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
    public class ZirconiumWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Zirconium Wand");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
			Item.crit = 1;
			Item.width = 28;
            Item.height = 34;
            Item.useTime = 42;
            Item.useAnimation = 42;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.mana = 7;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Magic.ZirconProj>();
            Item.shootSpeed = 10;

            Item.DamageType = DamageClass.Magic;
        }

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.ZirconiumBar>(), 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}