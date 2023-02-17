using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SupernovaMod.Content.Tiles
{
    public class RingForge : ModTile
    {
        public override void SetStaticDefaults()
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
            AnimationFrameHeight = 34;
            AddMapEntry(new Color(120, 85, 60), name);
        }

        public override void KillMultiTile(int x, int y, int frameX, int frameY)
        {
            if (frameX == 0)
            {
                Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 48, 48, ModContent.ItemType<Items.Tiles.RingForge>());
            }
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
