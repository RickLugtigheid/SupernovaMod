using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace Supernova.Content.PreHardmode.Items.Tools
{
    public class VerglasPick : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Verglas Pick");
        }

        public override void SetDefaults()
        {   
            Item.damage = 12;
            Item.width = 24;
            Item.height = 24;

            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7;
            Item.value = Item.buyPrice(0, 3, 54);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.pick = 100;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.VerglasBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
