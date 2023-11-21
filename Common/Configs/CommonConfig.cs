using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SupernovaMod.Common.Configs
{
	public class CommonConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		#region Effect Settings
		[Header("Effects")]
		[DefaultValue(true)]
		public bool allowScreenShake;

		[DefaultValue(400)]
		public int maxParticles;
		#endregion

		#region UI Settings
		[Header("UI")]
		[DefaultValue(true)]
		public bool enableRingSlot;
		#endregion

		/*[Header("Misc")]
		[DefaultValue(false)]
		public bool debugMode;*/

		public override void OnChanged()
		{
			base.OnChanged();

			// TODO: Reset particle system (the size of the particles array)
		}
	}
}
