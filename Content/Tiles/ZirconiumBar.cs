using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace SupernovaMod.Content.Tiles
{
	public class ZirconiumBar : ModTile
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

			AddMapEntry(new Color(172, 56, 207), Language.GetText("MapObject.MetalBar")); // localized text for "Metal Bar"
			DustType = ModContent.DustType<Dusts.ZirconDust>();
		}
	}
}
