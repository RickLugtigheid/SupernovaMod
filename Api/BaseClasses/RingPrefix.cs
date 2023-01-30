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
		}
	}
}
