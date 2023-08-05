using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SupernovaMod.Common
{
	public static class SupernovaUtils
	{
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

		public static float Magnitude(this Vector2 self) => (float)Math.Sqrt(self.X * self.X + self.Y * self.Y);

		/// <summary>
		/// Moves an npc to a location smoothly
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="movementDistanceGateValue"></param>
		/// <param name="distanceFromDestination"></param>
		/// <param name="baseVelocity"></param>
		/// <param name="acceleration"></param>
		/// <param name="useSimpleFlyMovement"></param>
		public static void MoveNPCSmooth(NPC npc, float movementDistanceGateValue, Vector2 distanceFromDestination, float baseVelocity, float acceleration, bool useSimpleFlyMovement)
		{
			float lerpValue = Utils.GetLerpValue(movementDistanceGateValue, 2400f, distanceFromDestination.Length(), true);
			float minVelocity = distanceFromDestination.Length();
			if (minVelocity > baseVelocity)
			{
				minVelocity = baseVelocity;
			}
			Vector2 maxVelocity = distanceFromDestination / 24f;
			float maxVelocityCap = baseVelocity * 3f;
			if (maxVelocity.Length() > maxVelocityCap)
			{
				maxVelocity = Utils.SafeNormalize(distanceFromDestination, Vector2.Zero) * maxVelocityCap;
			}
			Vector2 desiredVelocity = Vector2.Lerp(Utils.SafeNormalize(distanceFromDestination, Vector2.Zero) * minVelocity, maxVelocity, lerpValue);
			if (useSimpleFlyMovement)
			{
				npc.SimpleFlyMovement(desiredVelocity, acceleration);
				return;
			}
			npc.velocity = desiredVelocity;
		}
		/// <summary>
		/// Moves an projectile to a location smoothly
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="movementDistanceGateValue"></param>
		/// <param name="distanceFromDestination"></param>
		/// <param name="baseVelocity"></param>
		/// <param name="acceleration"></param>
		/// <param name="useSimpleFlyMovement"></param>
		public static void MoveProjectileSmooth(Projectile proj, float movementDistanceGateValue, Vector2 distanceFromDestination, float baseVelocity, float acceleration)
		{
			float lerpValue = Utils.GetLerpValue(movementDistanceGateValue, 2400f, distanceFromDestination.Length(), true);
			float minVelocity = distanceFromDestination.Length();
			if (minVelocity > baseVelocity)
			{
				minVelocity = baseVelocity;
			}
			Vector2 maxVelocity = distanceFromDestination / 24f;
			float maxVelocityCap = baseVelocity * 3f;
			if (maxVelocity.Length() > maxVelocityCap)
			{
				maxVelocity = Utils.SafeNormalize(distanceFromDestination, Vector2.Zero) * maxVelocityCap;
			}
			Vector2 desiredVelocity = Vector2.Lerp(Utils.SafeNormalize(distanceFromDestination, Vector2.Zero) * minVelocity, maxVelocity, lerpValue);
			proj.velocity = desiredVelocity;
		}
		/// <summary>
		/// Get the required rotation to rotate to the target position.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="targetPosition"></param>
		/// <returns></returns>
		public static float GetTargetLookRotation(this Entity entity, Vector2 targetPosition)
		{
			Vector2 direction = entity.Center - targetPosition;
			float rotation = (float)Math.Atan2(direction.Y, direction.X);
			return rotation - (float)Math.PI * 0.5f;
		}
		/// <summary>
		/// Get the required rotation to rotate to the target position.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="targetPosition"></param>
		/// <returns></returns>
		public static float GetTargetLookRotation(this Vector2 position, Vector2 targetPosition)
		{
			Vector2 direction = position - targetPosition;
			float rotation = (float)Math.Atan2(direction.Y, direction.X);
			return rotation - (float)Math.PI * 0.5f;
		}

		public static bool DoesFitInCone(Vector2 point, Vector2 coneCenter, float coneLength, float coneRotation, float maximumAngle)
		{
			Vector2 spinningpoint = point - coneCenter;
			float num = spinningpoint.RotatedBy(0f - coneRotation).ToRotation();
			if (num < 0f - maximumAngle || num > maximumAngle)
			{
				return false;
			}
			return spinningpoint.Length() < coneLength;
		}

		public static void StartThunderStorm()
		{
			int num = 86400;
			int num2 = num / 24;
			Main.rainTime = (double)Main.rand.Next(num2 * 8, num);
			if (Utils.NextBool(Main.rand, 3))
			{
				Main.rainTime += (double)Main.rand.Next(0, num2);
			}
			if (Utils.NextBool(Main.rand, 4))
			{
				Main.rainTime += (double)Main.rand.Next(0, num2 * 2);
			}
			if (Utils.NextBool(Main.rand, 5))
			{
				Main.rainTime += (double)Main.rand.Next(0, num2 * 2);
			}
			if (Utils.NextBool(Main.rand, 6))
			{
				Main.rainTime += (double)Main.rand.Next(0, num2 * 3);
			}
			if (Utils.NextBool(Main.rand, 7))
			{
				Main.rainTime += (double)Main.rand.Next(0, num2 * 4);
			}
			if (Utils.NextBool(Main.rand, 8))
			{
				Main.rainTime += (double)Main.rand.Next(0, num2 * 5);
			}

			Main.cloudBGActive = 1f;
			Main.numCloudsTemp = 200;
			Main.numClouds = Main.numCloudsTemp;
			Main.windSpeedCurrent = (float)Main.rand.Next(50, 75) * 0.01f;
			Main.windSpeedTarget = Main.windSpeedCurrent;
			Main.weatherCounter = Main.rand.Next(3600, 18000);
			Main.maxRaining = 0.89f;
			Main.StartRain();
		}
	}
}
