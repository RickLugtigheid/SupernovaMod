using Microsoft.Xna.Framework;
using SupernovaMod.Content.Projectiles.BaseProjectiles;
using Terraria;
using Terraria.ID;

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