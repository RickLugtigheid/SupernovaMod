using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SupernovaMod.Common.Configs
{
	public class SupernovaModConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("Effects")]
		[DefaultValue(true)]
		public bool allowScreenShake;

		[Header("Misc")]
		[DefaultValue(false)]
		public bool debugMode;
	}
}
