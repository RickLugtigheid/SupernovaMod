using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace Supernova.Content.Items.Weapons.Throwing
{
    public class ZirconiumKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;

            DisplayName.SetDefault("Zirconium Trowing Knive");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999; // Makes it so the weapon stacks.
            Item.damage = 14;
            Item.crit = 4;
            Item.knockBack = 4;
            Item.useStyle = 1;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.width = 30;
            Item.height = 30;
            Item.consumable = true; // Makes it so one is taken from stack after use.
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.value = 64;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.ZirconiumKnifeProj>();

            Item.DamageType = DamageClass.Throwing;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(25);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.ZirconiumBar>());
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
