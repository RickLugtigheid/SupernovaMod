using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Common
{
	public static class PlayerUtils
	{
		/// <inheritdoc cref="Player.AddBuff(int, int, bool, bool)"/>
		public static void AddBuff<T>(this Player player, int timeToAdd, bool quiet = true, bool foodHack = false) where T : ModBuff => player.AddBuff(ModContent.BuffType<T>(), timeToAdd, quiet, foodHack);
	}
}
