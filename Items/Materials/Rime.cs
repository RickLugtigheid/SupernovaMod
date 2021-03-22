using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Supernova.Items.Materials
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
            ItemID.Sets.ItemIconPulse[item.type] = true;
        }

        // TODO -- Velocity Y smaller, post NewItem?
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.value = 1200;
            item.rare = Rarity.Orange;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale);
        }
    }
}
