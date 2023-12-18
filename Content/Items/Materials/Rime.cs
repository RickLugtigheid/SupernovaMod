using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Materials
{
    public class Rime : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }

        // TODO -- Velocity Y smaller, post NewItem?
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = 1200;
            Item.rare = ItemRarityID.Orange;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale);
        }
    }
}
