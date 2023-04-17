using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SupernovaMod.Common.Systems
{
	public class DownedSystem : ModSystem
	{
		/* Bosses Downed */
		// PreHardmode
		//
		public static bool downedHarbingerOfAnnihilation = false;
		public static bool downedFlyingTerror = false;
		public static bool downedStormSovereign = false;

		private void ResetDowned()
		{
			downedHarbingerOfAnnihilation = false;
			downedFlyingTerror = false;
			downedStormSovereign = false;
		}

		public override void OnWorldLoad()
		{
			ResetDowned();
		}
		public override void OnWorldUnload()
		{
			ResetDowned();
		}

		public override void SaveWorldData(TagCompound tag)
		{
			var downed = new List<string>();

			if (downedHarbingerOfAnnihilation) downed.Add("HarbingerOfAnnihilation");
			if (downedFlyingTerror) downed.Add("FlyingTerror");
			if (downedStormSovereign) downed.Add("StormSovereign");

			tag.Add("downed", downed);
		}

		public override void LoadWorldData(TagCompound tag)
		{
			var downed = tag.GetList<string>("downed");

			downedHarbingerOfAnnihilation = downed.Contains("HarbingerOfAnnihilation");
			downedFlyingTerror = downed.Contains("FlyingTerror");
			downedStormSovereign = downed.Contains("StormSovereign");
		}

		public override void NetSend(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = downedHarbingerOfAnnihilation;
			flags[1] = downedFlyingTerror;
			flags[2] = downedStormSovereign;

			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			downedHarbingerOfAnnihilation = flags[0];
			downedFlyingTerror = flags[1];
			downedStormSovereign = flags[2];

			base.NetReceive(reader);
		}
	}
}
