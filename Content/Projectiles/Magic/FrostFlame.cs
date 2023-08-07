using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class FrostFlame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Frost Fire");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;  //Set the hitbox width
            Projectile.height = 12; //Set the hitbox height
            Projectile.friendly = true;  //Tells the game whether it is friendly to players/friendly npcs or not
            Projectile.ignoreWater = true;  //Tells the game whether or not projectile will be affected by water
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 2; //Tells the game how many enemies it can hit before being destroyed, -1 infinity
            Projectile.timeLeft = 150;  //The amount of time the projectile is alive for  
            Projectile.extraUpdates = 3;
            Projectile.alpha = 255;
		}

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.45f / 255f, (255 - Projectile.alpha) * 0.05f / 255f);   //this is the light colors
            if (Projectile.timeLeft > 64)
                Projectile.timeLeft = 64;
            if (Projectile.ai[0] > 12f)  //this defines where the flames starts
            {
                if (Main.rand.NextBool(3))     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.IceTorch, Projectile.velocity.X * 1.2f, Projectile.velocity.Y * 1.2f, 130, default, 4.75f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 2.5f;
                }
            }
            else
                Projectile.ai[0]++;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 180);   //this make so when the projectile/flame hit a npc, gives it the buff  onfire , 80 = 3 seconds
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }
}
