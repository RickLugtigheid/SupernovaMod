using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class CarnageScepterProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Carnage Scepter");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            AIType = 521;
        }
        public override void OnKill(int timeLeft)
        {
            //SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 109);
            SoundEngine.PlaySound(SoundID.Item2, Projectile.position);
            if (Projectile.owner == Main.myPlayer)
            {
                int num220 = Main.rand.Next(2, 4);
                for (int num221 = 0; num221 < num220; num221++)
                {
                    Vector2 value17 = new Vector2(Main.rand.Next(-75, 76), Main.rand.Next(-75, 76));
                    value17.Normalize();
                    value17 *= Main.rand.Next(20, 51) * 0.1f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, value17.X, value17.Y, ModContent.ProjectileType<CarnageScepterProj2>(), (int)(Projectile.damage * .7f), 1f, Projectile.owner, 0, Main.rand.Next(-45, 1));
                }
            }
        }
        public override void AI()
        {
            //this is projectile dust
            int DustID2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.BloodDust>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 20, default, 1);
            Main.dust[DustID2].noGravity = true;
        }
    }
}
