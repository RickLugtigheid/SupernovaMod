using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Supernova.Content.Tiles
{
    public class ZirconiumOreTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            ItemDrop = ModContent.ItemType<Items.Tiles.ZirconiumOre>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Zirconium");
            AddMapEntry(new Color(187, 78, 181), name);
            MinPick = 20;
        }
    }
}