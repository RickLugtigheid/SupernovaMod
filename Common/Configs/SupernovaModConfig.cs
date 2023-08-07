using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SupernovaMod.Common.Configs
{
	public class SupernovaModConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("Misc")]
		[DefaultValue(false)]
		public bool debugMode;
	}
}
