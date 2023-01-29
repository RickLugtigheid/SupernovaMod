using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.Global.Projectiles.Magic
{
    public class GlowGunProj : ModProjectile
    {
		public const float MAX_DEVIATION_ANGLE = 1.2f;
		public const float HOMING_RANGE = 150;
		public const float HOMING_ANGLE = .83f;
		public const int TIME_LEFT = 210;

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glowing Projectile");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = TIME_LEFT;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            AIType = 521;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            // Vanilla explosions do less damage to Eater of Worlds in expert mode, so for balance we will too.
            if (Main.expertMode)
            {
                if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail)
                {
                    damage /= 5;
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Vector2 position = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item24, position);

            // Truffle Explosion effect
            for (int j = 0; j <= Main.rand.Next(3, 7); j++)
            {
                Vector2 velocity = (Projectile.velocity * Main.rand.Next(3, 6)).RotatedByRandom(360);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position.X, Projectile.position.Y, velocity.X, velocity.Y, ProjectileID.Mushroom, (int)(Projectile.damage * .75f), 3 * 1, Projectile.owner, 1, 0);
            }
        }

		public NPC FindTarget()
		{
			float bestScore = 0f;
			NPC bestTarget = null;
			for (int i = 0; i < 200; i++)
			{
				NPC potentialTarget = Main.npc[i];
				if (potentialTarget.CanBeChasedBy(null, false))
				{
					float distance = potentialTarget.Distance(base.Projectile.Center);
					float angle = base.Projectile.velocity.AngleFrom(potentialTarget.Center - base.Projectile.Center);
					float extraDistance = (float)(potentialTarget.width / 2 + potentialTarget.height / 2);
					if (distance - extraDistance < HOMING_RANGE && angle < HOMING_ANGLE / 2f && (Collision.CanHit(base.Projectile.Center, 1, 1, potentialTarget.Center, 1, 1) || extraDistance >= distance))
					{
						float attemptedScore = EvaluateTarget(distance - extraDistance, angle / 2f);
						if (attemptedScore > bestScore)
						{
							bestTarget = potentialTarget;
							bestScore = attemptedScore;
						}
					}
				}
			}
			return bestTarget;
		}
		public float EvaluateTarget(float distance, float angle)
		{
			return 1f - distance / HOMING_RANGE * 0.5f + (1f - Math.Abs(angle) / (HOMING_ANGLE / 2f)) * 0.5f;
		}

		private NPC _target = null;
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, (Color.GreenYellow * 0.8f).ToVector3() * 0.5f);
			
			_target = FindTarget();

			if (_target != null )
			{
				float distanceFromTarget = (_target.Center - Projectile.Center).Length();
				// Update projectile rotation
				//
				Projectile.rotation = Utils.AngleTowards(Projectile.rotation, Utils.ToRotation(_target.Center - base.Projectile.Center), 0.07f * (float)Math.Pow((double)(1f - distanceFromTarget / HOMING_RANGE), 2.0));

				// Update projectile velocity
				//
				Projectile.velocity *= 0.99f;
				Projectile.velocity = Utils.ToRotationVector2(Projectile.rotation) * Projectile.velocity.Length();
			}
		}

		internal Color ColorFunction(float completionRatio)
		{
			float fadeOpacity = (float)Math.Sqrt((double)(1f - completionRatio));
			return Utils.MultiplyRGB(Color.DeepSkyBlue, Color.Blue) * fadeOpacity;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = ModContent.Request<Texture2D>(Texture, AssetRequestMode.ImmediateLoad).Value;
			Main.spriteBatch.Draw
			(
				texture,
				new Vector2
				(
					Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
					Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				Projectile.rotation,
				texture.Size() * 0.5f,
				Projectile.scale,
				SpriteEffects.None,
				0f
			);
			return false;
		}
	}
}
