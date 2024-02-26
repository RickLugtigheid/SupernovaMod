using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using SupernovaMod.Common.Players;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace SupernovaMod.Content.Projectiles.Summon
{
    public class CarnageOrb : ModProjectile
    {
        private const int MAX_REGEN_VALUE = 200;
		public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        public override void AI()
        {
			CheckActive();

            int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width - 10, Projectile.height - 10, DustID.Blood, Projectile.velocity.X * 20, Projectile.velocity.Y * 20, 70, default, 1.25f);
            Main.dust[DustID2].noGravity = true;

            OrbitAI();
			ProjectileBlockingAI();

			Player owner = Main.player[Projectile.owner];
			ref float regenValue = ref Projectile.localAI[1];
            if (regenValue > 0)
            {
				regenValue--;
                owner.UpdateLifeRegen();

				SoundEngine.PlaySound(SoundID.Item15);
				Vector2 dustPos = owner.Center + new Vector2(30, 0).RotatedByRandom(MathHelper.ToRadians(360));
				Vector2 diff = owner.Center - dustPos;
				diff.Normalize();

				Dust.NewDustPerfect(dustPos, DustID.LifeDrain, diff * 2).noGravity = true;
			}
        }

        private void OrbitAI()
        {
			Player owner = Main.player[Projectile.owner];
			if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<CarnageOrb>()] > 1)
			{
				Projectile.timeLeft = 0;
			}

			//Factors for calculations
			double deg = Projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
			double rad = deg * (Math.PI / 180); //Convert degrees to radians
			double dist = 75; //Distance away from the player

			/*Position the player based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.   */
			Projectile.position.X = owner.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
			Projectile.position.Y = owner.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

			//Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
			Projectile.ai[1] += 4;
		}
		private void ProjectileBlockingAI()
		{
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj == null || !proj.active)
				{
					continue;
				}
				if (!proj.hostile)
				{
					continue;
				}
				// Block the projectile when colliding
				//
				if (Collision.CheckAABBvAABBCollision(
					Projectile.position, new Vector2(Projectile.width, Projectile.height),
					proj.position, new Vector2(proj.width, proj.height)
				))
				{
					// We should only block projectiles that have no penetration left
					//
					if (proj.penetrate == 0)
					{
						proj.Kill();
					}
					else if (proj.penetrate != -1)
					{
						proj.penetrate--;
					}
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
            if (Projectile.localAI[1] < MAX_REGEN_VALUE)
			    Projectile.localAI[1] += 120;
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (info.PvP)
            {
				if (Projectile.localAI[1] < MAX_REGEN_VALUE)
					Projectile.localAI[1] += 120;
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
            modifiers.HitDirectionOverride = Vector2.Distance(Projectile.Center, Main.player[Projectile.owner].Center) > 0 ? -1 : 1;
		}

		public void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            ArmorPlayer modPlayer = player.GetModPlayer<ArmorPlayer>();
            if (player.dead)
            {
                modPlayer.carnageArmor = false;
            }
            if (modPlayer.carnageArmor)
            {
                Projectile.timeLeft = 2;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Projectile.velocity.X, Projectile.velocity.Y, Scale: 1.5f);
            }
            base.OnKill(timeLeft);
        }

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects spriteEffects = 0;
			if (Projectile.spriteDirection == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Color color24 = Projectile.GetAlpha(lightColor);
			Color color25 = Lighting.GetColor((int)(Projectile.position.X + Projectile.width * 0.5) / 16, (int)((Projectile.position.Y + Projectile.height * 0.5) / 16.0));
			Texture2D texture2D3 = TextureAssets.Projectile[Projectile.type].Value;
			int num156 = TextureAssets.Projectile[Projectile.type].Value.Height / 1;
			int y3 = num156 * (int)Projectile.frameCounter;
			Rectangle rectangle = new Rectangle(0, y3, texture2D3.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			int num157 = 8;
			int num158 = 2;
			int num159 = 1;
			float num160 = 0f;
			int num161 = num159;
			Main.spriteBatch.Draw(texture2D3, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, Projectile.width, Projectile.height), color24, Projectile.rotation, new Vector2(Projectile.width, Projectile.height) / 2f, Projectile.scale, spriteEffects, 0f);
			while (num158 > 0 && num161 < num157 || num158 < 0 && num161 > num157)
			{
				Color color26 = Projectile.GetAlpha(color25);
				float num162 = num157 - num161;
				if (num158 < 0)
				{
					num162 = num159 - num161;
				}
				color26 *= num162 / (ProjectileID.Sets.TrailCacheLength[Projectile.type] * 1.5f);
				Vector2 value4 = Projectile.oldPos[num161];
				float num163 = Projectile.rotation;
				Main.spriteBatch.Draw(texture2D3, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(rectangle), color26, num163 + Projectile.rotation * num160 * (num161 - 1) * -(float)spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin2, Projectile.scale, spriteEffects, 0f);
				num161 += num158;
			}
			return true;
		}
	}
}
