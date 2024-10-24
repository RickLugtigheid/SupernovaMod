using Microsoft.Xna.Framework;
using SupernovaMod.Api.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Melee
{
    public class MicroMagnetSphere : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.MagnetSphereBall}";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.MagnetSphereBall);
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.timeLeft = 120;
            Projectile.scale = .65f;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 25)
            {
                SearchForTargets(Main.player[Projectile.owner], out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter, out NPC target);
                if (foundTarget && distanceFromTarget < 300)
                {
                    Shoot(targetCenter, 10);
                    Projectile.ai[0] = 0;
                    Projectile.timeLeft = 0;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor.A = 125;
            return base.PreDraw(ref lightColor);
        }

        private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter, out NPC? target)
        {
            // Starting search distance
            distanceFromTarget = 700f;
            targetCenter = Projectile.position;
            foundTarget = false;
            target = null;

            // This code is required if your minion weapon has the targeting feature
            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                // Reasonable distance away so it doesn't target across multiple screens
                if (between < 2000f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                    target = npc;
                }
            }

            if (!foundTarget)
            {
                // This code is required either way, used for finding a target
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                        // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                        // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                        bool closeThroughWall = between < 100f;

                        if ((closest && inRange || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                            target = npc;
                        }
                    }
                }
            }

            // friendly needs to be set to true so the minion can deal contact damage
            // friendly needs to be set to false so it doesn't damage things like target dummies while idling
            // Both things depend on if it has a target or not, so it's just one assignment here
            // You don't need this assignment if your minion is shooting things instead of dealing contact damage
            Projectile.friendly = foundTarget;
        }

        private void Shoot(Vector2 targetCenter, float shotSpeed)
        {
            Vector2 Velocity = Mathf.VelocityFPTP(Projectile.Center, targetCenter, shotSpeed);
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, Velocity.X, Velocity.Y, ProjectileID.MagnetSphereBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
            Main.projectile[proj].DamageType = Projectile.DamageType;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= Main.rand.Next(10, 20); i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, DustID.MagnetSphere, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 20, default);
            }
        }
    }
}
