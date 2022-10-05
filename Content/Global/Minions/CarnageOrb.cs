﻿using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Supernova.Common.Players;

namespace Supernova.Content.Global.Minions
{
    public class CarnageOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Orb");
        }
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            //Projectile.minion = true;
            //Projectile.minionSlots = 0;
            Projectile.DamageType = DamageClass.Default;
        }

        /*public static bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.ModProjectile != null)
                return projectile.ModProjectile.OnTileCollide(oldVelocity);
            return true;
        }*/

        public override void AI()
        {
            int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width - 10, Projectile.height - 10, ModContent.DustType<Dusts.BloodDust>(), Projectile.velocity.X * 12, Projectile.velocity.Y * 12, 70, default(Color), .8f);
            Main.dust[DustID2].noGravity = true;

            #region orbit
            //Making player variable "p" set as the projectile's owner
            Player p = Main.player[Projectile.owner];
            if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<CarnageOrb>()] > 1)
            {
                Projectile.timeLeft = 0;
            }

            //Factors for calculations
            double deg = (double)Projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
            double rad = deg * (Math.PI / 180); //Convert degrees to radians
            double dist = 90; //Distance away from the player

            /*Position the player based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.     */
            Projectile.position.X = p.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = p.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

            //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
            Projectile.ai[1] += 3;
            #endregion

            #region Lifesteal
            /*for (int i = 0; i < 200; i++)
            {
                //Enemy NPC variable being set
                NPC target = Main.npc[i];

                //Getting the shooting trajectory
                float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                float shootToY = target.position.Y - Projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                //If the distance between the projectile and the live target is active
                if (distance < 240f && !target.friendly && target.netID != NPCID.TargetDummy && target.active)
                {
                    Projectile.ai[0]++;
                    if (Projectile.ai[0] > 20) //Assuming you are already incrementing this in AI outside of for loop
                    {
                        float shootX = target.position.X + (float)target.width * 0.5f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), shootX, target.position.Y, 0, 0, ProjectileID.SoulDrain, 1, 0, Projectile.owner, 0f, 0f);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), shootX, target.position.Y, 0, 0, ProjectileID.SpiritHeal, 1, 0, Projectile.owner, 0f, 1);
                        Projectile.ai[0] = 0;
                    }
                }
            }*/
            #endregion
            CheckActive();
        }
        public void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            AccessoryPlayer modPlayer = player.GetModPlayer<AccessoryPlayer>();
            if (player.dead)
            {
                modPlayer.hasMinionCarnageOrb = false;
            }
            if (modPlayer.hasMinionCarnageOrb)
            {
                Projectile.timeLeft = 2;
            }
            if (!player.HasBuff(ModContent.BuffType<Buffs.Minion.CarnageOrbBuff>()))
            {
                modPlayer.hasMinionCarnageOrb = false;
            }
        }
    }
}
