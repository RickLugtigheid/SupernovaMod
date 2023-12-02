using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Tools
{
    public class ZirconiumPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Zirconium Pickaxe");
        }

        public override void SetDefaults()
        {
            Item.damage = 6; // Base Damage of the Weapon
            Item.width = 24; // Hitbox Width
            Item.height = 24; // Hitbox Height

            Item.useTime = 17; // Speed before reuse
            Item.useAnimation = 17; // Animation Speed
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1.5f; // Weapon Knockbase: Higher means greater "launch" distance
            Item.value = 25500; // 10 | 00 | 00 | 00 : Platinum | Gold | Silver | Bronze
            Item.rare = ItemRarityID.Green; // Item Tier
            Item.UseSound = SoundID.Item1; // Sound effect of item on use 
            Item.autoReuse = true; // Do you want to torture people with clicking? Set to false
			Item.useTurn = true;

			Item.pick = 50; // Pick Power - Higher Value = Better
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 3);
            recipe.AddIngredient(ModContent.ItemType<Materials.ZirconiumBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
