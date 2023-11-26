using Terraria.ID;
using Terraria.Localization;
using Terraria.ObjectData;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Tiles.Furniture
{
	public class FlyingTerrorBossTrophy : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(120, 85, 60), Language.GetText("MapObject.Trophy"));
			DustType = DustID.WoodFurniture;
		}
	}
}
