using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SupernovaMod.Common.Systems;

namespace SupernovaMod.Content.Projectiles.Thrown
{
    public class ZirconiumKnifeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zirconium Trowing Knive");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ThrowingKnife);
            AIType = ProjectileID.ThrowingKnife;
			Projectile.DamageType = GlobalModifiers.DamageClass_ThrowingRanged;
		}
		public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            // Spawn dust on kill
            //
            for (int i = 0; i < 8; i++)
            {
                int dustID = Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, ModContent.DustType<Dusts.ZirconDust>(), Projectile.velocity.X * .3f, Projectile.velocity.Y * .8f, 0, default, Main.rand.NextFloat(.3f, .6f));
                Main.dust[dustID].velocity.RotatedByRandom(MathHelper.ToRadians(180));
            }

            int item = 0;
            // Give the projectile a 7% chance to drop it's item after death
            //
            if (Main.rand.NextFloat() < 0.07f)
            {
                item = Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.position, ModContent.ItemType<Items.Weapons.Throwing.ZirconiumKnife>());
            }

            if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
            {
                NetMessage.SendData(MessageID.KillProjectile);
            }
        }
    }
}
