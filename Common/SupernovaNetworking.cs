using Terraria;
using Terraria.ID;

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
