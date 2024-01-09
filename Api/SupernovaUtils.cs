using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Utilities;

namespace SupernovaMod.Api
{
    public static class SupernovaUtils
    {
		/// <summary>
		/// Creates an explosion using the projectile. Calls <see cref="Projectile.Damage" />, then <see cref="Projectile.Kill" />.
		/// </summary>
		/// <param name="proj">The projectile</param>
		/// <param name="width">The width of the explosion</param>
		/// <param name="height">The height of the explosion</param>
		/// <param name="knockback">The knockback of the explosion, if null, uses the projectile's knockback</param>
		public static void CreateExplosion(this Projectile proj, int width, int height, float? knockback = null, bool killProjectile = true)
		{
			proj.penetrate = -1;
			proj.tileCollide = false;
			proj.alpha = 255;
			proj.Resize(width, height);
			proj.knockBack = knockback ?? proj.knockBack;

			proj.Damage();
			if (killProjectile)
            {
				proj.Kill();
			}
		}
		/// <summary> 
		/// Has a <paramref name="chancePercent"/> chance to return true. 
		/// </summary> 
		/// <param name="rand"></param> 
		/// <param name="chancePercent">The percentage chance of returning True. (.5 for 50%)</param> 
		/// <returns></returns> 
		public static bool NextChance(this UnifiedRandom rand, float chancePercent) => rand.NextFloat(1f) < chancePercent;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
		public static Item GetActiveItem(this Player player)
		{
			if (!Main.mouseItem.IsAir)
			{
				return Main.mouseItem;
			}
			return player.HeldItem;
		}
		public static Tile ParanoidTileRetrieval(int x, int y)
		{
			if (!WorldGen.InWorld(x, y, 0))
			{
				return default(Tile);
			}
			return Main.tile[x, y];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetNamespacePath(Type type) => type.Namespace.Replace('.', '/') + "/" + type.Name;
		/// <summary>
		/// Tries to load a texture at <see cref="Type.Namespace"/> of <paramref name="type"/>
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool TryLoadTexture(Type type, out Texture2D texture)
        {
			string texturePath = type.Namespace.Replace('.', '/') + "/" + type.Name;

            try
            {
                texture = ModContent.Request<Texture2D>(texturePath, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                return true;
			}
            catch
            {
                texture = null;
                return false;
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
            origin.X *= player.direction;
            origin.Y *= player.gravDir;
            player.itemRotation = desiredRotation;
            if (flipAngle)
            {
                player.itemRotation *= player.direction;
            }
            else if (player.direction < 0)
            {
                player.itemRotation += 3.1415927f;
            }
            Vector2 consistentAnchor = player.itemRotation.ToRotationVector2() * (spriteSize.X / -2f - 10f) * player.direction - origin.RotatedBy(player.itemRotation, default);
            Vector2 offsetAgain = spriteSize * -0.5f;
            Vector2 finalPosition = desiredPosition + offsetAgain + consistentAnchor;
            if (stepDisplace)
            {
                int frame = player.bodyFrame.Y / player.bodyFrame.Height;
                if (frame > 6 && frame < 10 || frame > 13 && frame < 17)
                {
                    finalPosition -= Vector2.UnitY * 2f;
                }
            }
            player.itemLocation = finalPosition;
        }

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
                maxVelocity = distanceFromDestination.SafeNormalize(Vector2.Zero) * maxVelocityCap;
            }
            Vector2 desiredVelocity = Vector2.Lerp(distanceFromDestination.SafeNormalize(Vector2.Zero) * minVelocity, maxVelocity, lerpValue);
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
                maxVelocity = distanceFromDestination.SafeNormalize(Vector2.Zero) * maxVelocityCap;
            }
            Vector2 desiredVelocity = Vector2.Lerp(distanceFromDestination.SafeNormalize(Vector2.Zero) * minVelocity, maxVelocity, lerpValue);
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
        public static Vector2 LookAt(this Vector2 vector2, Vector2 targetPosition)
        {
			Vector2 direction = vector2 - targetPosition;
			float rotation = (float)Math.Atan2(direction.Y, direction.X);
			return Vector2.One.RotatedBy(rotation - (float)Math.PI * 0.5f) * vector2;
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

        public static void StartThunderStorm()
        {
            int num = 86400;
            int num2 = num / 24;
            Main.rainTime = Main.rand.Next(num2 * 8, num);
            if (Main.rand.NextBool(3))
            {
                Main.rainTime += Main.rand.Next(0, num2);
            }
            if (Main.rand.NextBool(4))
            {
                Main.rainTime += Main.rand.Next(0, num2 * 2);
            }
            if (Main.rand.NextBool(5))
            {
                Main.rainTime += Main.rand.Next(0, num2 * 2);
            }
            if (Main.rand.NextBool(6))
            {
                Main.rainTime += Main.rand.Next(0, num2 * 3);
            }
            if (Main.rand.NextBool(7))
            {
                Main.rainTime += Main.rand.Next(0, num2 * 4);
            }
            if (Main.rand.NextBool(8))
            {
                Main.rainTime += Main.rand.Next(0, num2 * 5);
            }

            Main.cloudBGActive = 1f;
            Main.numCloudsTemp = 200;
            Main.numClouds = Main.numCloudsTemp;
            Main.windSpeedCurrent = Main.rand.Next(50, 75) * 0.01f;
            Main.windSpeedTarget = Main.windSpeedCurrent;
            Main.weatherCounter = Main.rand.Next(3600, 18000);
            Main.maxRaining = 0.89f;
            Main.StartRain();
        }
        /// <summary>
        /// Spawns a localized chat text for <paramref name="key"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="color"></param>
		public static void NewLocalizedText(string key, Color? color = null)
        {
            if (color == null)
            {
                color = Color.White;
            }

			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.NewText(Language.GetTextValue(key), color);
				return;
			}
			if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key, Array.Empty<object>()), color.Value, -1);
			}
        }
	}
}
