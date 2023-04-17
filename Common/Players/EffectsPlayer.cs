using Microsoft.Xna.Framework;
using SupernovaMod.Content.Items.Accessories;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Players
{
	public class EffectsPlayer : ModPlayer
	{
		/// <summary>
		/// TODO: Add to mod config
		/// </summary>
		private float ScreenShakeConfigValue = 1;

		public bool effectScreenShake = false;
		private int _shakeValue = 0;

		public void StartScreenShake(int value)
		{
			_shakeValue = value;
			effectScreenShake = true;
		}

		public override void ModifyScreenPosition()
		{
			if (effectScreenShake)
			{
				Main.screenPosition.Y += Main.rand.Next(-_shakeValue, _shakeValue) * ScreenShakeConfigValue;
				Main.screenPosition.X += Main.rand.Next(-_shakeValue, _shakeValue) * ScreenShakeConfigValue;
				if (_shakeValue > 0) { _shakeValue--; }
			}
		}
	}
}
