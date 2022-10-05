using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace Supernova.Content.Global.Dusts
{
    public class TerrorDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.3f; // Mulitiply the Velocity on both X and Y by a float (Example 0.6f)
            dust.noGravity = true; // Dust doesn't fall to the ground
            dust.noLight = true; // Dust doesn't emit light
            dust.scale *= 2f; // The scale of the graphic (Default: 1f)
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity; // Moves the dust according to the velocity.
            //dust.rotation += dust.velocity.X * 0.2f; // Will rotate the dust based on Velocity.
            dust.scale *= 0.90f; // Will decrease the scale over time
            if (dust.scale < 0.5f)
            {
                dust.active = false;
            }
            return false;
        }
    }
}