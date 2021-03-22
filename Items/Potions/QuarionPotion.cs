using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Potions
{
    public class QuarionPotion: ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quarion Potion");
            Tooltip.SetDefault("Grealy increces damage and speed" +
                "\nYou lose half of your health");
        }

        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item3;                //this is the sound that plays when you use the item
            item.useStyle = 2;                 //this is how the item is holded when used
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.maxStack = 30;                 //this is where you set the max stack of item
            item.consumable = true;           //this make that the item is consumable when used
            item.width = 20;
            item.height = 28;
            item.value = 100;
            item.rare = 1;
            item.buffTime = 20000;    //this is the buff duration        20000 = 6 min
            item.buffType = mod.BuffType("QuarionBuff");    //this is where you put your Buff name
            return;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(mod.GetItem("QuarionShard"));
            recipe.AddIngredient(ItemID.Blinkroot);
            recipe.AddIngredient(ItemID.LeadOre);
            recipe.AddTile(13);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(mod.GetItem("QuarionShard"));
            recipe.AddIngredient(ItemID.Blinkroot);
            recipe.AddIngredient(ItemID.IronOre);
            recipe.AddTile(13);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
