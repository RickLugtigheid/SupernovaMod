using System;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;

namespace Supernova
{
    public static class Calc
    {
        public static float ToPrecent(float value, int percent)
        {
            float p = percent / 100;
            return value * p;
        }
        /// <summary>
        /// Gets a random position in the world between 2 positions
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <returns></returns>
        public static Vector2 RandomPosition(Vector2 pos1, Vector2 pos2)
        {
            return new Vector2(Main.rand.Next((int)pos1.X, (int)pos2.X) + 1, Main.rand.Next((int)pos1.Y, (int)pos2.Y) + 1);
        }

        /// <summary>
        /// Calculates random spread
        /// </summary>
        /// <param name="speedX"></param>
        /// <param name="speedY"></param>
        /// <param name="angle"></param>
        /// <param name="angle_extra"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static Vector2[] RandomSpread(float speedX, float speedY, int angle, float angle_extra, int num)
        {
            var posArray = new Vector2[num];
            float spread = (float)(angle * angle_extra);
            float baseSpeed = (float)System.Math.Sqrt(speedX * speedX + speedY * speedY);
            double baseAngle = System.Math.Atan2(speedX, speedY);
            double randomAngle;
            for (int i = 0; i < num; ++i)
            {
                randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                posArray[i] = new Vector2(baseSpeed * (float)System.Math.Sin(randomAngle), baseSpeed * (float)System.Math.Cos(randomAngle));
            }
            return (Vector2[])posArray;
        }

        public static Vector2 VelocityFPTP(Vector2 pos1, Vector2 pos2, float speed)
        {
            Vector2 move = pos2 - pos1;
            //speed2 = speed * 0.5;
            return move * (speed / (float)Math.Sqrt(move.X * move.X + move.Y * move.Y));
        }
    }
    public static class Screen
    {
        public static void Write(string message, Color color)
        {
            if (Main.netMode != 2)
            {
                Main.NewText(Language.GetTextValue(message), color);
            }
            else
            {
                NetworkText text = NetworkText.FromKey(message);
                NetMessage.BroadcastChatMessage(text, color);
            }
        }
    }
}
