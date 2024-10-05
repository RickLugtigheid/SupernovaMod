using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Dusts
{
    public class BlackFlamesDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.15f; // Mulitiply the Velocity on both X and Y by a float (Example 0.6f)
            dust.noGravity = true; // Dust doesn't fall to the ground
            dust.noLight = true; // Dust doesn't emit light
            dust.scale *= 2f; // The scale of the graphic (Default: 1f)
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity; // Moves the dust according to the velocity.
            dust.rotation += dust.velocity.X * 0.2f; // Will rotate the dust based on Velocity.
            dust.scale *= 0.92f; // Will decrease the scale over time
            if (dust.scale < 0.25f)
            {
                dust.active = false;
            }
            return false;
        }
    }
}
