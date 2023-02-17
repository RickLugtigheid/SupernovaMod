using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.Items.Tiles
{
    public class RingForge : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Ring Forge");
            Tooltip.SetDefault("A forge where you can make all the rings of power.");
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 26;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.value = Item.buyPrice(0, 5, 40, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Content.Tiles.RingForge>();
            Item.maxStack = 1;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronBar, 58);
            recipe.AddIngredient(ItemID.LavaBucket, 1);
            //recipe.anyIronBar = true;
            recipe.acceptedGroups = new() { RecipeGroupID.IronBar };
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
