using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Supernova.Projectiles.Minions
{
    public class CarnageOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Orb");
        }
        public override void SetDefaults()
        {
            projectile.width = 38;
            projectile.height = 38;
            projectile.aiStyle = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.minion = true;
            projectile.minionSlots = 0;
        }

        public static bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.modProjectile != null)
                return projectile.modProjectile.OnTileCollide(oldVelocity);
            return true;
        }

        public override void AI()
        {
            int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width - 10, projectile.height - 10, ModContent.DustType<Dusts.BloodDust>(), projectile.velocity.X * 5, projectile.velocity.Y * 5, 70, default(Color));
            Main.dust[DustID2].noGravity = true;

            #region orbit
            //Making player variable "p" set as the projectile's owner
            Player p = Main.player[projectile.owner];
            if (Main.player[projectile.owner].ownedProjectileCounts[mod.ProjectileType("eyeProj")] > 1)
            {
                projectile.timeLeft = 0;
            }
            //Factors for calculations
            double deg = (double)projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
            double rad = deg * (Math.PI / 180); //Convert degrees to radians
            double dist = 90; //Distance away from the player

            /*Position the player based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.     */
            projectile.position.X = p.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width / 2;
            projectile.position.Y = p.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height / 2;

            //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
            projectile.ai[1] += 2;
            #endregion

            #region Lifesteal
            for (int i = 0; i < 200; i++)
            {
                //Enemy NPC variable being set
                NPC target = Main.npc[i];

                //Getting the shooting trajectory
                float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
                float shootToY = target.position.Y - projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                //If the distance between the projectile and the live target is active
                if (distance < 240f && !target.friendly && target.netID != NPCID.TargetDummy && target.active)
                {
                    projectile.ai[0]++;
                    if (projectile.ai[0] > 20) //Assuming you are already incrementing this in AI outside of for loop
                    {
                        float shootX = target.position.X + (float)target.width * 0.5f;
                        Projectile.NewProjectile(shootX, target.position.Y, 0, 0, ProjectileID.SoulDrain, 1, 0, projectile.owner, 0f, 0f);
                        Projectile.NewProjectile(shootX, target.position.Y, 0, 0, ProjectileID.SpiritHeal, 1, 0, projectile.owner, 0f, 1);
                        projectile.ai[0] = 0;
                    }
                }
            }

            #endregion
            CheckActive();
        }
        public void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            SupernovaPlayer modPlayer = player.GetModPlayer<SupernovaPlayer>();
            if (player.dead)
            {
                modPlayer.minionCarnageOrb = false;
            }
            if (modPlayer.minionCarnageOrb)
            {
                projectile.timeLeft = 2;
            }
            if (!player.HasBuff(mod.BuffType("CarnageOrbBuff")))
            {
                modPlayer.minionCarnageOrb = false;
            }
        }
    }
}
