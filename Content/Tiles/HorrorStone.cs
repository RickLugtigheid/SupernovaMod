using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Tiles
{
    public class HorrorStone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
			LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(187, 78, 181), name);
            DustType = DustID.Chlorophyte;
            //MinPick = 20;
        }
    }
}