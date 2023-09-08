using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Common;

namespace SupernovaMod.Content.Npcs.HarbingerOfAnnihilation.Projectiles
{
    public class HarbingerMissile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Harbinger Missile");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 45;
            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 6000;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.light = 0.2f;
            AIType = ProjectileID.DeathLaser;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = new Color(180, 180, 180, 245);
            //Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            Vector2 drawPos = Projectile.oldPos[6] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - 6) / (float)Projectile.oldPos.Length);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);

            Vector2 drawOrigin2 = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            Vector2 drawPos2 = Projectile.oldPos[3] - Main.screenPosition + drawOrigin2 + new Vector2(0f, Projectile.gfxOffY);
            Color color2 = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - 3) / (float)Projectile.oldPos.Length);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos2, null, color2, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            return true;
        }
    }
}
