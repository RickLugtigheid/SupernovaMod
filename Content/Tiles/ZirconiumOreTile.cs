using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Tiles
{
    public class ZirconiumOreTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            RegisterItemDrop(ModContent.ItemType<Items.Placeable.ZirconiumOre>());
			LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(187, 78, 181), name);
			DustType = ModContent.DustType<Dusts.ZirconDust>();
			MinPick = 20;
        }
    }
}