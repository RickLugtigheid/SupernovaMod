using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Players
{
	public static class SupernovaPlayerUtils
	{
		public static SupernovaPlayer Supernova(this Player player) => player.GetModPlayer<SupernovaPlayer>();
	}
	public class SupernovaPlayer : ModPlayer
	{
	}
}