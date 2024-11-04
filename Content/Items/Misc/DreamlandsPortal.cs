using SubworldLibrary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using SupernovaMod.Common.World.Dreamlands;

namespace SupernovaMod.Content.Items.Misc
{
    public class DreamlandsPortal : ModItem
    {
        public override void SetDefaults()
        {
            Item.useTurn = true;
            Item.width = 16;
            Item.height = 16;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 45;
            Item.UseSound = SoundID.MoonLord;
            Item.useAnimation = 45;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(100, 0, 0);
            Item.maxStack = 1;
        }

        public override bool? UseItem(Player player)
        {
            if (SubworldSystem.IsActive<DreamlandsSubworld>())
            {
                return false;
            }

            SubworldSystem.Enter<DreamlandsSubworld>();
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            if (SubworldSystem.IsActive<DreamlandsSubworld>())
            {
                SubworldSystem.Exit();
                return true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.WoodPlatform, 200);
            recipe.AddIngredient(ItemID.Campfire, 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
