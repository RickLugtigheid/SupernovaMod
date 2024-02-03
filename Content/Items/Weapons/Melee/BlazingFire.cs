using SupernovaMod.Content.Projectiles.Melee.Swordstaffs;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class BlazingFire : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Blazing Fire");
            // Tooltip.SetDefault("Creates a fire aura around you.");
        }
        public override void SetDefaults()
        {
			Item.damage = 25;
			Item.knockBack = 8;
			Item.useAnimation = (Item.useTime = 25);
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.channel = true;
			Item.autoReuse = true;
			Item.shootSpeed = 14f;
			Item.shoot = ModContent.ProjectileType<BlazingFireProj>();
			Item.width = 128;
			Item.height = 140;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = SoundID.DD2_SkyDragonsFurySwing;
			Item.value = Item.buyPrice(0, 5, 40, 0);
			Item.rare = ItemRarityID.Orange;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddIngredient(ItemID.Obsidian, 7);
            recipe.AddTile(TileID.Hellforge);
            recipe.Register();
        }
    }
}