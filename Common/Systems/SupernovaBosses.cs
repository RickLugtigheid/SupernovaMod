using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Supernova.Common.Systems
{
	public class SupernovaBosses : ModSystem
	{
		/* Bosses Downed */
		// PreHardmode
		//
		public static bool downedHarbingerOfAnnihilation = false;
		public static bool downedFlyingTerror = false;
		public static bool downedStormSovereign = false;

		public override void OnWorldLoad()
		{
			downedHarbingerOfAnnihilation = false;
			downedFlyingTerror = false;
			downedStormSovereign = false;

			base.OnWorldLoad();
		}

		public override void SaveWorldData(TagCompound tag)
		{
			var downed = new List<string>();

			if (downedHarbingerOfAnnihilation) downed.Add("HarbingerOfAnnihilation");
			if (downedFlyingTerror) downed.Add("FlyingTerror");
			if (downedStormSovereign) downed.Add("StormSovereign");

			base.SaveWorldData(tag);
		}

		public override void LoadWorldData(TagCompound tag)
		{
			var downed = tag.GetList<string>("downed");

			downedHarbingerOfAnnihilation = downed.Contains("HarbingerOfAnnihilation");
			downedFlyingTerror = downed.Contains("FlyingTerror");
			downedStormSovereign = downed.Contains("StormSovereign");

			base.LoadWorldData(tag);
		}

		public override void NetSend(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = downedHarbingerOfAnnihilation;
			flags[0] = downedFlyingTerror;
			flags[0] = downedStormSovereign;

			writer.Write(flags);

			base.NetSend(writer);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			downedHarbingerOfAnnihilation = flags[0];
			downedFlyingTerror = flags[0];
			downedStormSovereign = flags[0];

			base.NetReceive(reader);
		}
	}
}
