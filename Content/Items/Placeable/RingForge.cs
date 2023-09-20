using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Placeable
{
	public class RingForge : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 26;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 5, 40, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.RingForge>();
            Item.maxStack = 1;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronBar, 58);
            recipe.AddIngredient(ItemID.LavaBucket, 1);
            recipe.acceptedGroups = new() { RecipeGroupID.IronBar };
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
