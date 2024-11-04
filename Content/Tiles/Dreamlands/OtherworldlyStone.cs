using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Tiles.Dreamlands
{
    public class OtherworldlyStone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;

            LocalizedText tileName = CreateMapEntryName();
            AddMapEntry(new Color(187, 78, 181), tileName);
            DustType = DustID.Chlorophyte;
        }
    }
}
