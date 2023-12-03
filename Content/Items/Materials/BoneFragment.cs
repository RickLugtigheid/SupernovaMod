using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Materials
{
    public class BoneFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;

            // DisplayName.SetDefault("Bone Fragment");
            // Tooltip.SetDefault("Drops from any zombies after the Brain of Cthulhu/Eater of Worlds is defeated");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = Item.buyPrice(0, 0, 2, 32);
            Item.rare = ItemRarityID.Green;
        }
    }
}
