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
		private bool CanShakeScreen = true;

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
