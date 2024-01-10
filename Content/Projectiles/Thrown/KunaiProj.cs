using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SupernovaMod.Common.Systems;

namespace SupernovaMod.Content.Projectiles.Thrown
{
    public class KunaiProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Kunai");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ThrowingKnife);
            AIType = ProjectileID.ThrowingKnife;
            Projectile.penetrate = 1;
			Projectile.DamageType = GlobalModifiers.DamageClass_ThrowingRanged;
		}

		public override void OnKill(int timeLeft)
        {
            //SoundEngine.PlaySound(0, (int)Projectile.position.X, (int)Projectile.position.Y, 1, 1f, 0f);
            SoundEngine.PlaySound(SoundID.Dig);

            int item = 0;
            // Give the projectile a 7% chance to drop it's item after death
            //
            if (Main.rand.NextFloat() < 0.07f)
            {
                item = Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.position, Projectile.width, Projectile.height, ModContent.ItemType<Items.Weapons.Throwing.Kunai>(), 1, false, 0, false, false);
            }

            if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
            {
                NetMessage.SendData(MessageID.KillProjectile);
            }
        }
    }
}
