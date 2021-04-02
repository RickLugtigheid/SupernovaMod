using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova
{
	public class GlobalNpc : GlobalNPC
	{
		// All loot related
		public override void NPCLoot(NPC npc)
		{
			/* Misc Drops */

			// Drop from any zombie
			if (-27 >= npc.netID && npc.netID >= -37 || npc.netID == 3 || 186 >= npc.netID && npc.netID >= 189)
				if (Main.rand.Next(4) == 0 && NPC.downedBoss2 == true)
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BoneFragment"), Main.rand.Next(3));

			/* Biome Drops */

			// Jungle drops
			if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneJungle)
			{
				if (Main.rand.Next(100) == 1)
					Item.NewItem(npc.getRect(), mod.ItemType("StaffOfThorns"));
			}
			// Glowing Mushroom drops
			if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneGlowshroom)
			{
				if (Main.rand.Next(64) == 0)
					Item.NewItem(npc.getRect(), mod.ItemType("BagOfFungus"));
			}
			// Snow Biome drops
			if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneSnow)
			{
				if (Main.rand.Next(5) == 0 && NPC.downedQueenBee == true)
					Item.NewItem(npc.getRect(), mod.ItemType("Rime"));
			}

			/* Event Drops */
			if (Main.bloodMoon == true)
				NPCBloodMoonLoot(npc);
		}
		public void NPCBloodMoonLoot(NPC npc)
		{
			/// Spawn blood schards
			if (Main.rand.Next(4) == 0)
				Item.NewItem(npc.getRect(), mod.ItemType("BloodShards"));
		}
	}
}
