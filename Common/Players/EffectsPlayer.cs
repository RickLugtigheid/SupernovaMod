using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Players
{
	public class EffectsPlayer : ModPlayer
	{
		/// <summary>
		/// The power of the sceen shake. The higher the value the stronger (and longer) the shake.
		/// </summary>
		public float ScreenShakePower = 0;

		public override void ModifyScreenPosition()
		{
			if (!ModContent.GetInstance<Configs.CommonConfig>().allowScreenShake)
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
