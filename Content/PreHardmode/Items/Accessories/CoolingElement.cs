using Supernova.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Accessories
{
    public class CoolingElement : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Cooling Element");
            Tooltip.SetDefault("Decreases ring cooldown by 7%\nWhen your ring is cooling down your movement speed increases by 10%");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 6, 0, 0);
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            RingPlayer.ringCooldownMulti -= 0.07f;
            if (player.HasBuff(ModContent.BuffType<Global.Buffs.RingCooldown>()))
			{
                player.moveSpeed *= 1.1f;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IceBlock, 43);
            recipe.AddIngredient(ModContent.ItemType<Materials.VerglasBar>(), 7);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
