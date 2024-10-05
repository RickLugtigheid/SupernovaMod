using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Common
{
	public static class SupernovaNetworking
	{
		public static void SyncWorldData()
		{
			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.WorldData);
			}
		}
	}
}
