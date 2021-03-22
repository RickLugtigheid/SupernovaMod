using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supernova
{
	public static class Rarity
	{
		/// <summary>
		/// The lowest tier. Only "junk" fishing items have this as a base rarity: Tin Can, Old Shoe, and Seaweed.
		/// </summary>
		public const int Gray = -1;
		/// <summary>
		/// Items without a rarity value specified in Terraria's game code default to this tier. 
		/// </summary>
		public const int White = 0;
		/// <summary>
		/// Weapons and armor crafted from early ores, along with early dropped/looted items like the Shackle and Lucky Horseshoe.
		/// </summary>
		public const int Blue = 1;
		/// <summary>
		/// Midway pre-Hardmode items. These are mostly looted, dropped, or purchased (non-craftable) items.
		/// </summary>
		public const int Green = 2;
		/// <summary>
		/// Late-stage pre-Hardmode and some early hardmode items: Weapons and armor made of Hellstone, Jungle and Underground Jungle items, Underworld items.
		/// </summary>
		public const int Orange = 3;
		/// <summary>
		/// Early Hardmode items, including those crafted from the six Hardmode ores spawned from destroying Altars, and item drops from early and/or common Hardmode enemies.
		/// </summary>
		public const int LigtRed = 4;
		/// <summary>
		/// Mid-Hardmode (pre-Plantera) items, including those acquired after defeating mechanical bosses. Also includes the more expensive Hardmode NPC purchases.
		/// </summary>
		public const int Pink = 5;
		/// <summary>
		/// A smaller tier consisting of the rarest pre-Plantera items, mostly purchased or dropped. Also some higher-tier Tinkerer's Workshop combinations.
		/// </summary>
		public const int LightPurple = 6;
		/// <summary>
		/// tems acquired around Plantera and Golem, and the Hardmode Underground Jungle. Chlorophyte tools and weapons.
		/// </summary>
		public const int Lime = 7;
		/// <summary>
		/// Items acquired or crafted from loot obtained in the post-Plantera Dungeon, Biome Chest items; Certain Mimic drops with high modifiers; And Drops from the late-hardmode events
		/// </summary>
		public const int Yellow = 8;
		/// <summary>
		/// This smaller tier contains early items acquired from the Lunar Events
		/// </summary>
		public const int Cyan = 9;
		/// <summary>
		/// Items crafted at the Ancient Manipulator from Lunar Fragments and/or Luminite.
		/// </summary>
		public const int Red = 10;
		/// <summary>
		/// This tier consists of Cyan and Red items that have high-level modifiers
		/// </summary>
		public const int Purple = 11;
		/// <summary>
		/// This special tier contains Expert mode-exclusive items obtained by opening Treasure Bags
		/// </summary>
		public const int Rainbow = -12;
		/// <summary>
		/// This special tier contains only "quest items" to be turned over to quest-giving NPCs 
		/// </summary>
		public const int Amber = -11;
	}
}
