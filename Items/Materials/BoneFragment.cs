using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class BoneFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bone Fragment");
            Tooltip.SetDefault("Drops from any zombies after the Brain of Cthulhu/Eater of Worlds is defeated");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.value = Item.buyPrice(0, 0, 2, 32);
            item.rare = Rarity.Green;
        }
    }
}
