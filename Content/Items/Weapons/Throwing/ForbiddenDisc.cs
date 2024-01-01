using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Throwing
{
    public class ForbiddenDisc : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 44;
            Item.crit = 3;
            Item.noMelee = true;
            Item.maxStack = 1;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 11;
            Item.useAnimation = 11;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.shootSpeed = 16;
            Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.ForbiddenDiscProj>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Throwing;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe.AddIngredient(ModContent.ItemType<DiscOfTheDesert>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}