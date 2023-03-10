using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Common
{
	public static class SupernovaUtils
	{
		/// <summary>
		/// Gets the key(s) name(s) formatted for use in tooltips.
		/// </summary>
		/// <param name="hotkey"></param>
		/// <returns></returns>
		public static string GetKeyTooltip(this ModKeybind hotkey)
		{
			try
			{
				if (hotkey == null || Main.dedServ)
				{
					return string.Empty;
				}

				Queue<string> boundKeys = new Queue<string>(hotkey.GetAssignedKeys(Terraria.GameInput.InputMode.Keyboard));

				// Check if any keys where bound
				//
				if (boundKeys.Count == 0)
				{
					return "[NONE]";
				}

				StringBuilder tooltipBuilder = new StringBuilder();
				// Add our first key
				tooltipBuilder.Append(boundKeys.Dequeue());

				// Add all additional keys
				//
				while (boundKeys.TryDequeue(out string keyName))
				{
					tooltipBuilder.Append(" / ").Append(keyName);
				}
				return tooltipBuilder.ToString();
			}
			catch (System.Exception e)
			{
				Console.WriteLine("SupernovaUtils.GetKeyTooltip(): Error '" + e.Message + "'");
				return string.Empty;
			}
		}


		/// <summary>
		/// Checks if the chest is empty.
		/// </summary>
		/// <param name="chest"></param>
		/// <returns>If our Chest is empty</returns>
		public static bool IsEmptyChest(this Chest chest)
		{
			for (int i = 0; i < chest.item.Length; i++)
			{
				if (chest.item[i] == null || chest.item[i].IsAir) { continue; }
				return false;
			}
			return true;
		}

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
