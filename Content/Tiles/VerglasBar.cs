using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace SupernovaMod.Content.Tiles
{
	public class VerglasBar : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileShine[Type] = 1100;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(209, 205, 255), Language.GetText("MapObject.MetalBar")); // localized text for "Metal Bar"
			DustType = DustID.Frost;
		}
	}
}
