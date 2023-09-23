using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using SupernovaMod.Api.Effects;
using SupernovaMod.Content.Effects.Particles;
using SupernovaMod.Content.Projectiles.BaseProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Melee
{
    public class VerglasSlash : SwordSlashProj
    {
		protected override Color BackDarkColor { get; } = new Color(209, 205, 255); // Original Excalibur color: Color(180, 160, 60)
		protected override Color MiddleMediumColor { get; } = new Color(101, 122, 202); // Original Excalibur color: Color(255, 255, 80)
		protected override Color FrontLightColor { get; } = new Color(248, 242, 255); // Original Excalibur color: Color(255, 240, 150)

		protected override int DustType1 { get; } = DustID.IceTorch;
		protected override Color DustColor1 => Color.Lerp(Color.CornflowerBlue, Color.Blue, Main.rand.NextFloat() * 1f);
		protected override Color DustColor2 { get; } = Color.CornflowerBlue;

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Excalibur);
			Projectile.aiStyle = -1;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.Frostburn, Main.rand.Next(3, 7) * 60);

			//ParticleSystem.SpawnParticle(new Effects.Particles.ImpactSpark(60));
			/*for (int i = 0; i < 8; i++)
			{
				Vector2 vel = Main.rand.NextFloat(6.28f).ToRotationVector2() * 4;
				vel.Normalize();
				vel = vel.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
				vel *= Main.rand.NextFloat(2, 5);
				Effects.Particles.ImpactSpark line = new Effects.Particles.ImpactSpark(target.Center - (vel * 5), vel, Color.Purple, new Vector2(0.25f, Main.rand.NextFloat(0.75f, 1.75f)), 70);
				line.lifeTime = 30;
				ParticleSystem.SpawnParticle(line);
			}*/
			OnHitEffects(target.Center + Main.rand.NextVector2Circular(target.width / 2, target.height / 2));

		}
		private void OnHitEffects(Vector2 effectPosition)
		{
			if (Main.dedServ)
				return;

			// Impact star particle
			//
			ParticleSystem.SpawnParticle(new StarParticle(effectPosition, Vector2.Zero, Color.Lerp(Color.White, MiddleMediumColor, 0.5f), .65f, 20));
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (info.PvP)
			{
				target.AddBuff(BuffID.Frostburn, Main.rand.Next(3, 7) * 60);
			}
		}
	}
}