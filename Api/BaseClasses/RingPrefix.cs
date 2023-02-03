using Terraria;
using Terraria.ModLoader;
using Supernova.Common.Players;

namespace Supernova.Api
{
	public abstract class RingPrefix : ModPrefix
	{
		public override PrefixCategory Category => PrefixCategory.Accessory;

		// Check if item is a ring
		//
		public override bool CanRoll(Item item)
		{
			return RingPlayer.ItemIsRing(item);

			// EXAMPLE: https://github.com/tieeeeen1994/tModLoader-ArmorModifiers/tree/d734a643a20b4aeb527004ea9dc8a0d83d04c9d6
		}
	}
}
