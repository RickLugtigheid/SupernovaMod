using System;
using Terraria;
using Terraria.Utilities;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Common.World
{
    public class WorldGenSystem
    {
        public static void CustomTileRunner(UnifiedRandom rand, Point origin, float startStength, float stopStrength, int stepsCount, Vector2 startVelocity, Action<int, int> action)//, DitherType ditherForm = DitherType.None)
        {
            float stepsRemaining = stepsCount;
            float totalSteps = stepsCount;
            double strength = startStength;
            Vector2 position = Utils.ToVector2(origin);
            Vector2 velocity = ((startVelocity == Vector2.Zero) ? Utils.RandomVector2(rand, -1f, 1f) : startVelocity);
            while (stepsRemaining > 0f && strength > 0.0)
            {
                strength = startStength * (stepsRemaining / totalSteps);
                if (strength < (double)stopStrength)
                {
                    break;
                }
                stepsRemaining -= 1f;
                double halfStrength = strength * 0.5;
                int num = Math.Max(1, (int)((double)position.X - halfStrength));
                int minTileY = Math.Max(1, (int)((double)position.Y - halfStrength));
                int maxTileX = Math.Min(Main.maxTilesX, (int)((double)position.X + halfStrength));
                int maxTileY = Math.Min(Main.maxTilesY, (int)((double)position.Y + halfStrength));
                float randomFloat = Utils.NextFloat(WorldGen.genRand);
                for (int x = num; x < maxTileX; x++)
                {
                    for (int y = minTileY; y < maxTileY; y++)
                    {
                        bool ditherSkip = false;
                        //if (ditherForm == DitherType.Diamond)
                        //{
                        //    ditherSkip = (double)(Math.Abs((float)x - position.X) + Math.Abs((float)y - position.Y)) >= halfStrength;
                        //}
                        //if (ditherForm == DitherType.Circle)
                        //{
                        //    ditherSkip = (double)Vector2.DistanceSquared(new Vector2((float)x, (float)y), position) >= halfStrength * halfStrength;
                        //}
                        //if (ditherForm == DitherType.HorizontalElipse)
                        //{
                        //    Vector2 val = (new Vector2((float)x, (float)y) - position) * new Vector2(1f, 1.3f + randomFloat * 0.7f);
                        //    ditherSkip = (double)((Vector2)(ref val)).LengthSquared() >= halfStrength * halfStrength;
                        //}
                        if (!ditherSkip)
                        {
                            action(x, y);
                        }
                    }
                }
                int strengthScale = (int)(strength / 50.0) + 1;
                stepsRemaining -= (float)strengthScale;
                position += velocity;
                for (int i = 0; i < strengthScale; i++)
                {
                    position += velocity;
                    velocity += Utils.RandomVector2(rand, -0.5f, 0.5f);
                }
                velocity += Utils.RandomVector2(rand, -0.5f, 0.5f);
                velocity = Vector2.Clamp(velocity, -Vector2.One, Vector2.One);
            }
        }
    }
}
