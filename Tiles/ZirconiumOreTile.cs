using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Supernova.Tiles
{
    public class ZirconiumOreTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            drop = mod.ItemType("ZirconiumOre");
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Zirconium");
            AddMapEntry(new Color(187, 78, 181), name);
            minPick = 20;
        }
    }
}