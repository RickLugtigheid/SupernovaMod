using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Players
{
	public class EffectsPlayer : ModPlayer
	{
		/// <summary>
		/// TODO: Add to mod config
		/// </summary>
		private bool CanShakeScreen = true;
		/// <summary>
		/// The power of the sceen shake. The higher the value the stronger (and longer) the shake.
		/// </summary>
		public float ScreenShakePower = 0;

		public override void ModifyScreenPosition()
		{
			if (!CanShakeScreen)
			{
				return;
			}
			if (ScreenShakePower > 0)
			{
				Main.screenPosition += Main.rand.NextVector2Circular(ScreenShakePower, ScreenShakePower);
				ScreenShakePower = MathHelper.Clamp(ScreenShakePower - 0.185f, 0f, 20f);
			}
		}
	}
}
