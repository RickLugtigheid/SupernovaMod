using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Tiles
{
    public class HorrorStone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
			Main.tileStone[Type] = true;
			Main.tileMergeDirt[Type] = false;
            AddMapEntry(Color.DarkGreen);
            DustType = DustID.Chlorophyte;
            //MinPick = 20;
        }
    }
}