using SupernovaMod.Api;
using SupernovaMod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class CoolingElement : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Cooling Element");
            // Tooltip.SetDefault("8% decreased ring cooldown time\n10% increased movement speed when your ring is cooling down");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = BuyPrice.RarityGreen;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            ResourcePlayer resourcePlayer = player.GetModPlayer<ResourcePlayer>();
			resourcePlayer.ringCoolRegen -= 0.08f;

            if (player.HasBuff(ModContent.BuffType<Buffs.Cooldowns.RingCooldown>()))
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
