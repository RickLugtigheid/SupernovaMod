﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Supernova.Tiles
{
    public class RingForge : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            //TileObjectData.newTile.CoordinateHeights = new[] { 16, 24 };
            //TileObjectData.newTile.StyleHorizontal = true;
            //TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Ring Forge");
            animationFrameHeight = 34;
            AddMapEntry(new Color(120, 85, 60), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            if (frameX == 0)
                Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("RingForge"));
        }
		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
            frameCounter++;
            if (frameCounter > 4)
            {
                frameCounter = 0;
                frame++;
                frame %= 12;
            }
        }
	}
}
