using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using SupernovaMod.Api.Effects;
using System.Collections.Generic;
using Terraria.Audio;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class ShockSurgeProj : ModProjectile
    {
		private const int MAX_RANGE = 400;
		public override string Texture => Supernova.GetTexturePath("InvisibleProjectile");
		public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lightning");
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 4;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1;  //The amount of time the projectile is alive for
        }

        public static bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.ModProjectile != null)
                return projectile.ModProjectile.OnTileCollide(oldVelocity);
            return true;
        }

        private Vector2 _point = Vector2.Zero;

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
			//Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.45f / 255f, (255 - Projectile.alpha) * 0.05f / 255f);   //this is the light colors

			Player player = Main.player[Projectile.owner];
			if (_point == Vector2.Zero)
            {
				//Projectile.Center = Collision.AnyCollision(Projectile.position, Main.player[Projectile.owner].Center - Main.MouseWorld, 1, 1);
				Projectile.Center = Main.MouseWorld;
				_point = Main.player[Projectile.owner].Center;
				SearchForTargets(player, out bool foundTarget, out float distFromTarget, out Vector2 targetCenter);

                if (foundTarget)
                {
					Projectile.Center = targetCenter;
					Projectile.timeLeft = 4;
				}
			}

			DrawDust.Electricity(_point, Projectile.position, DustID.Electric, 1, 80);
        }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			_hitNPCs.Add(target);
			SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Projectile.position);

			// Spawn dust
			//
			for (int x = 0; x < 3; x++)
			{
				int dust = Dust.NewDust(target.Center, 25, 25, DustID.Electric, Main.rand.Next(-2, 2), -Main.rand.Next(1, 4), 0, default, Main.rand.NextFloat(.75f, 1));
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity *= Main.rand.NextFloat(1, 1.2f);
			}

			if (_hitNPCs.Count == 0)
            {
                return;
            }

			Projectile.timeLeft = 4;
			Player player = Main.player[Projectile.owner];
            _point = _hitNPCs[_hitNPCs.Count - 1].Center;
			SearchForTargets(player, out bool foundTarget, out float distFromTarget, out Vector2 targetCenter);

			if (foundTarget)
			{
				Projectile.position = targetCenter;
                Projectile.damage = (int)(Projectile.damage * .85f);
			}
		}

		private List<NPC> _hitNPCs = new List<NPC>();
		protected virtual void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
		{
			// Starting search distance
			distanceFromTarget = 300;
			targetCenter = Projectile.position;
			foundTarget = false;

			// Find a target
            //
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];

				if (!_hitNPCs.Contains(npc) && npc.CanBeChasedBy())
				{
					float between = Vector2.Distance(npc.Center, Projectile.Center);
					bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
					bool inRange = between < distanceFromTarget;
					if (between > MAX_RANGE)
					{
						inRange = false;
					}
					bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
					// Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
					// The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
					bool closeThroughWall = between < 100f;

					if ((closest && inRange || !foundTarget) && (lineOfSight || closeThroughWall))
					{
						distanceFromTarget = between;
						targetCenter = npc.Center;
						foundTarget = true;
					}
				}
			}
		}
	}
}
