using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Melee
{
    public class BlazingFireProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blazing Fire");
        }
        public override void SetDefaults()
        {
            Projectile.width = 125;     //Set the hitbox width
            Projectile.height = 125;       //Set the hitbox height
            Projectile.friendly = true;    //Tells the game whether it is friendly to players/friendly npcs or not
            Projectile.penetrate = -1;    //Tells the game how many enemies it can hit before being destroyed. -1 = never
            Projectile.tileCollide = false; //Tells the game whether or not it can collide with a tile
            Projectile.ignoreWater = true; //Tells the game whether or not projectile will be affected by water        
            Projectile.DamageType = DamageClass.Melee;

            Projectile.Size *= 1.75f;
        }

        public override void AI()
        {
            //-------------------------------------------------------------Sound-------------------------------------------------------
            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)//this is the proper sound delay for this type of weapon
            {
                SoundEngine.PlaySound(SoundID.DD2_SkyDragonsFurySwing /*2*/, Projectile.Center);    //this is the sound when the weapon is used
                Projectile.soundDelay = 45;    //this is the proper sound delay for this type of weapon
            }
            //-----------------------------------------------How the projectile works---------------------------------------------------------------------
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner)
            {
                if (!player.channel || player.noItems || player.CCed)
                {
                    Projectile.Kill();
                }
            }
            Lighting.AddLight(Projectile.Center, 1f, 0.6f, 0f);     //this is the projectile light color R, G, B (Red, Green, Blue)
            Projectile.Center = player.MountedCenter;
            Projectile.position.X += player.width / 2 * player.direction;  //this is the projectile width sptrite direction from the playr
            Projectile.spriteDirection = player.direction;
            Projectile.rotation += 0.3f * player.direction; //this is the projectile rotation/spinning speed
            if (Projectile.rotation > MathHelper.TwoPi)
            {
                Projectile.rotation -= MathHelper.TwoPi;
            }
            else if (Projectile.rotation < 0)
            {
                Projectile.rotation += MathHelper.TwoPi;
            }
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = Projectile.rotation;

            // Add dust to the blade
            ApplyBladeDust();

            // Set the knockback to the correct direction
            //Projectile.knockBack *= Projectile.spriteDirection == 1 ? 1 : -1;
            //Projectile.direction = Projectile.spriteDirection;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = Projectile.spriteDirection == 1 ? 1 : -1;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        private void ApplyBladeDust()
        {
            for (int i = 0; i < 3; i++)
            {
                //Dust.NewDust(Projectile.Center * Projectile.rotation, Projectile.width / 2, Projectile.height / 4, DustID.Torch, velocity.X, velocity.Y);

                //Dust.NewDust(Projectile.Center + velocity, Projectile.width / 2, Projectile.height / 4, DustID.Torch, velocity.X, velocity.Y);
                Vector2 position = Projectile.Center + new Vector2(Projectile.width / 2 - i * 2, 0).RotatedBy(Projectile.rotation);
                Dust.NewDustPerfect(position, DustID.Torch);
            }
        }

        public override bool PreDraw(ref Color lightColor)  //this make the projectile sprite rotate perfectaly around the player
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire3, 120);
        }
    }
}