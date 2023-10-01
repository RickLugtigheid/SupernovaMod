using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Consumables.Potions
{
    public class QuarionPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

            // DisplayName.SetDefault("Quarion Potion");
            /* Tooltip.SetDefault("Grealy increases damage and speed" +
                "\nYou lose half of your health"); */
        }

        public override void SetDefaults()
        {
            Item.UseSound = SoundID.Item3;                //this is the sound that plays when you use the item
            Item.useStyle = 2;                 //this is how the item is holded when used
            Item.useTurn = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.maxStack = 30;                 //this is where you set the max stack of item
            Item.consumable = true;           //this make that the item is consumable when used
            Item.width = 20;
            Item.height = 28;
            Item.value = 100;
            Item.rare = ItemRarityID.Blue;
            Item.buffTime = 20000;    //this is the buff duration        20000 = 6 min
            Item.buffType = ModContent.BuffType<Buffs.Potions.QuarionBuff>();    //this is where you put your Buff name
            return;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<Materials.QuarionShard>());
            recipe.AddIngredient(ItemID.Blinkroot);
            recipe.AddIngredient(ItemID.LeadOre);
            recipe.AddTile(13);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<Materials.QuarionShard>());
            recipe.AddIngredient(ItemID.Blinkroot);
            recipe.AddIngredient(ItemID.IronOre);
            recipe.AddTile(13);
            recipe.Register();
        }
    }
}
