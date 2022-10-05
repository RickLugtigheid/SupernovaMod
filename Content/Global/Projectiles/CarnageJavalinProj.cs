using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Content.Global.Projectiles
{
    public class CarnageJavalinProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Javalin");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 19; // Spears use this AI Style.
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false; // Does the projectile stop if it has collided with a tile?
            Projectile.scale = 1.3f; // The scale of the projectile
            Projectile.hide = true; // Hides the projectile?
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 0; // The alpha value of the projectile
        }

        public float movementFactor
        {
            get { return Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }
        Vector2 startMousePos = Vector2.Zero;
        public override void AI()
        {
            if (startMousePos == Vector2.Zero) startMousePos = Main.MouseWorld;
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);

            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);

            if (!projOwner.frozen)
            {
                if (movementFactor == 0f)
                {
                    movementFactor = 3f;
                    Projectile.netUpdate = true;
                }else if(projOwner.itemAnimation == projOwner.itemAnimationMax /2)
                {
                    Vector2 diff = startMousePos - projOwner.Center;

                    diff.Normalize();
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position.X, Projectile.position.Y, diff.X * 16, diff.Y * 16, ProjectileID.NettleBurstRight, (int)(Projectile.damage * .45f), 2, Main.myPlayer, 0f, 0f);
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3)
                {
                    movementFactor -= 2.4f;
                } else
                {
                    movementFactor += 2.4f;
                }
            }
            Projectile.position += Projectile.velocity * movementFactor;

            if(projOwner.itemAnimation == 0)
            {
                Projectile.Kill();
            }
        }
    }
}
