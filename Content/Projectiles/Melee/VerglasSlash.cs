using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Content.Projectiles.BaseProjectiles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Melee
{
    public class VerglasSlash : SwordSlashProj
    {
		public override Color SparkleColor => new Color(10, 10, 10, 0);
		public override Color BigSparkleColor => new Color(3, 68, 116, 0);
		public override Color color1 => new Color(209, 205, 255);
		public override Color color2 => new Color(101, 122, 202);
		public override Color color3 => new Color(248, 242, 255);
		public override float scalemod => .8f;
		public override bool CanCutTile => true;

		public override int Dust1 => DustID.IceTorch;
		public override Color Dust1Color => Color.Lerp(Color.CornflowerBlue, Color.Blue, Main.rand.NextFloat() * 1f);
		public override int Dust2 => DustID.Ice;

		public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Verglas Splitter");
			Main.projFrames[Type] = 4;
		}
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.ownerHitCheck = true;
			Projectile.ownerHitCheckDistance = 300f;
			Projectile.scale = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.alpha = 255;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, Main.rand.Next(3, 7) * 60);

			/*if (Main.rand.NextBool(2))
			{
				for (int i = 0; i < 3; i++)
				{
					int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.position, (Vector2.One * 5).RotatedByRandom(5), ProjectileID.IceBolt, damage / 2, knockback / 2, Projectile.owner);
					Main.projectile[proj].DamageType = DamageClass.Melee;
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].hostile = false;
				}
			}*/
        }
	}

	public class VerglasSlash2 : SwordSlashProj
	{
		public override string Texture => "SupernovaMod/Content/Projectiles/Melee/VerglasSlash";
		public override Color SparkleColor => new Color(30, 30, 80, 0);
		public override Color BigSparkleColor => new Color(3, 68, 116, 0);
		public override Color color1 => new Color(209, 205, 255);
		public override Color color2 => new Color(101, 122, 202);
		public override Color color3 => new Color(248, 242, 255);
		public override float scalemod => .5f;
		public override bool CanCutTile => true;

		public override int Dust1 => DustID.IceTorch;
		public override Color Dust1Color => Color.Lerp(Color.CornflowerBlue, Color.Blue, Main.rand.NextFloat() * 1f);
		public override int Dust2 => DustID.Ice;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.ownerHitCheck = true;
			Projectile.ownerHitCheckDistance = 300f;
			Projectile.scale = 4;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.alpha = 255;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return false;
		}
	}
}