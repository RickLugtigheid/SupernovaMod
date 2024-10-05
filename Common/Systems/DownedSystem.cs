using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SupernovaMod.Common.Systems
{
	public class DownedSystem : ModSystem
	{
        // Pre-hardmode bosses
        //
        public static bool downedHarbingerOfAnnihilation = false;
		public static bool downedFlyingTerror = false;
		public static bool downedStormSovereign = false;

		// PreHardmode mini-bosses
		//
		public static bool downedBloodweaver = false;

		// Hardmode bosses
		//
		public static bool downedCosmicCollective = false;
		public static bool downedFallen = false;

		private static void ResetDowned()
		{
			downedHarbingerOfAnnihilation = false;
			downedFlyingTerror = false;
			downedStormSovereign = false;

			downedBloodweaver = false;

			downedCosmicCollective = false;
			downedFallen = false;
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

			if (downedBloodweaver) downed.Add("Bloodweaver");

			if (downedCosmicCollective) downed.Add("CosmicCollective");
			if (downedFallen) downed.Add("Fallen");

			tag.Add("downed", downed);
		}

		public override void LoadWorldData(TagCompound tag)
		{
			var downed = tag.GetList<string>("downed");

			downedHarbingerOfAnnihilation = downed.Contains("HarbingerOfAnnihilation");
			downedFlyingTerror = downed.Contains("FlyingTerror");
			downedStormSovereign = downed.Contains("StormSovereign");

			downedBloodweaver = downed.Contains("Bloodweaver");

            downedStormSovereign = downed.Contains("CosmicCollective");
            downedStormSovereign = downed.Contains("Fallen");
        }

        public override void NetSend(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = downedHarbingerOfAnnihilation;
			flags[1] = downedFlyingTerror;
			flags[2] = downedStormSovereign;

			flags[3] = downedBloodweaver;

            flags[4] = downedCosmicCollective;
            flags[5] = downedFallen;

            writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			downedHarbingerOfAnnihilation = flags[0];
			downedFlyingTerror = flags[1];
			downedStormSovereign = flags[2];

			downedBloodweaver = flags[3];

            downedCosmicCollective = flags[4];
            downedFallen = flags[5];

            base.NetReceive(reader);
		}
	}
}
