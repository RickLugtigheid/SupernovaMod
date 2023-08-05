using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class WoodenTarget : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Wooden Target");
            /* Tooltip.SetDefault("Increases trown and ranged damage by 4%" +
                                "\n Increases trown velocity by 3%"); */
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 2, 35, 89);
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 1000);
            recipe.AddIngredient(ItemID.StoneBlock, 200);
            recipe.AddIngredient(ItemID.Rope, 120);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.GetDamage(DamageClass.Throwing) += .04f;
            player.GetDamage(DamageClass.Ranged) += .04f;
            player.ThrownVelocity += 0.3f;
        }
    }
}
