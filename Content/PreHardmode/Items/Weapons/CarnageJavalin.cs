using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class CarnageJavalin : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Carnage Javalin");
        }

        public override void SetDefaults()
        {
            Item.damage = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 22;
            Item.useTime = 64;
            Item.shootSpeed = 4.18f; // The shoot speed for the spear projectile.
            Item.knockBack = 0.63f;
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.autoReuse = false; // Will auto reuse the item.
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<Global.Projectiles.CarnageJavalinProj>();

            Item.DamageType = DamageClass.Melee;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.BloodShards>(), 8);
            recipe.AddIngredient(ModContent.ItemType<Materials.BoneFragment>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
