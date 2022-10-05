using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Content.PreHardmode.Items.Materials
{
    public class Rime : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rime");
            Tooltip.SetDefault("Droped by creatures in the snow Biome");
            // ticksperframe, frameCount
                  //Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 2));
            //ItemID.Sets.AnimatesAsSoul[item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }

        // TODO -- Velocity Y smaller, post NewItem?
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = 1200;
            Item.rare = ItemRarityID.Orange;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale);
        }
    }
}
