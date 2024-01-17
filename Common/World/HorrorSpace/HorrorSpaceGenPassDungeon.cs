using System;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace SupernovaMod.Common.World.HorrorSpace
{
	public class HorrorSpaceGenPassDungeon : GenPass
	{
		public HorrorSpaceGenPassDungeon() : base("Dungeon", 3) { }

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			int dungeonLocation = GenVars.dungeonLocation;
			int num732 = (int)((HorrorSpaceSubworld.worldSurface + HorrorSpaceSubworld.rockLayer) / 2.0) + WorldGen.genRand.Next(-200, 200);
			int num733 = (int)((HorrorSpaceSubworld.worldSurface + HorrorSpaceSubworld.rockLayer) / 2.0) + 200;
			int num734 = num732;
			bool flag47 = false;
			for (int num735 = 0; num735 < 10; num735++)
			{
				if (WorldGen.SolidTile(dungeonLocation, num734 + num735))
				{
					flag47 = true;
					break;
				}
			}

			if (!flag47)
			{
				for (; num734 < num733 && !WorldGen.SolidTile(dungeonLocation, num734 + 10); num734++)
				{
				}
			}

			WorldGen.MakeDungeon(dungeonLocation, num734);
		}
	}
}
