using Microsoft.Xna.Framework;
using Terraria;

namespace Supernova.Common
{
	public static class SupernovaUtils
	{
		public static Player.CompositeArmStretchAmount ToStretchAmount(this float percent)
		{
			if (percent < 0.25f)
				return Player.CompositeArmStretchAmount.None;
			if (percent < 0.5f)
				return Player.CompositeArmStretchAmount.Quarter;
			if (percent < 0.75f)
				return Player.CompositeArmStretchAmount.ThreeQuarters;
			return 0;
		}
		public static void CleanHoldStyle(Player player, float desiredRotation, Vector2 desiredPosition, Vector2 spriteSize, Vector2? rotationOriginFromCenter = null, bool noSandstorm = false, bool flipAngle = false, bool stepDisplace = true)
		{
			if (noSandstorm)
			{
				player.sandStorm = false;
			}
			if (rotationOriginFromCenter == null)
			{
				rotationOriginFromCenter = new Vector2?(Vector2.Zero);
			}
			Vector2 origin = rotationOriginFromCenter.Value;
			origin.X *= (float)player.direction;
			origin.Y *= player.gravDir;
			player.itemRotation = desiredRotation;
			if (flipAngle)
			{
				player.itemRotation *= (float)player.direction;
			}
			else if (player.direction < 0)
			{
				player.itemRotation += 3.1415927f;
			}
			Vector2 consistentAnchor = Utils.ToRotationVector2(player.itemRotation) * (spriteSize.X / -2f - 10f) * (float)player.direction - Utils.RotatedBy(origin, (double)player.itemRotation, default(Vector2));
			Vector2 offsetAgain = spriteSize * -0.5f;
			Vector2 finalPosition = desiredPosition + offsetAgain + consistentAnchor;
			if (stepDisplace)
			{
				int frame = player.bodyFrame.Y / player.bodyFrame.Height;
				if ((frame > 6 && frame < 10) || (frame > 13 && frame < 17))
				{
					finalPosition -= Vector2.UnitY * 2f;
				}
			}
			player.itemLocation = finalPosition;
		}
	}
}
