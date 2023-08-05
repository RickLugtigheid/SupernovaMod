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
            //ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Tiles.ZirconiumOre>();
            RegisterItemDrop(ModContent.ItemType<Items.Tiles.ZirconiumOre>()); // TODO: Check if necessary
			LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Zirconium");
            AddMapEntry(new Color(187, 78, 181), name);
            MinPick = 20;
        }
    }
}