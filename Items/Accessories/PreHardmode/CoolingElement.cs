using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Supernova.Items.Accessories.PreHardmode
{
    public class CoolingElement : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cooling Element");
            Tooltip.SetDefault("Decreases ring cooldown by 7%\nWhen your ring is cooling down your movement speed increases by 10%");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 16));
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.AnimatesAsSoul[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 6, 0, 0);
            item.accessory = true;
            item.rare = 2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            SupernovaPlayer.ringCooldownDecrease -= 0.07f;
            if (player.HasBuff(mod.BuffType("RingCooldown")))
            {
                player.moveSpeed *= 1.1f;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IceBlock, 43);
            recipe.AddIngredient(mod.GetItem("VerglasBar"), 7);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
