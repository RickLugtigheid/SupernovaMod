using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using SupernovaMod.Content.Items.Rings.BaseRings;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace SupernovaMod.Content.Items.Rings
{
    public class BloodweaversRing : SupernovaRingItem
    {
		public override RingType RingType => RingType.Projectile;

		public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
			base.SetDefaults();
			Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 6, 0, 0);

			damage = 24;
		}
		public override int BaseCooldown => 60 * 140;
		public override bool CanRingActivate(RingPlayer player)
		{
			return true;
			//
			// TODO: Make multiplayer proof!
			//

			// Check if there is at least 1 target for the ring to use
			//
			for (int i = 0; i < 200; i++)
			{
				NPC target = Main.npc[i];

				if (target.CanBeChasedBy())
				{
					return true;
				}
			}
			return false;
		}
		public override void RingActivate(Player player, float ringPowerMulti)
        {
			SoundEngine.PlaySound(SoundID.Item14, player.Center);

			// Make sure this only runs for our client
			//
			if (player.whoAmI != Main.myPlayer)
			{
				return;
			}

			int targets = 0;
			for (int i = 0; i < 200; i++)
			{
				NPC target = Main.npc[i];

                // Check if hostile
                //
				if (target.CanBeChasedBy())
				{
					// Get a random starting velocity so not all projectiles will start the same direction.
					Vector2 startVelocity = new Vector2(
						Main.rand.Next(-10, 10),
						Main.rand.Next(-10, 10)
					);

					float shootX = target.position.X + (float)target.width * 0.5f;
					
                    // Spawn the damaging projectile
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), shootX, target.position.Y, 0, 0, ProjectileID.SoulDrain, damage, 0, Main.myPlayer, 0f, 0f);

                    // Spawn the healing projectile
                    int healAmount = (int)(damage * .4f);
					Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, startVelocity, ProjectileID.VampireHeal, 1, 0, player.whoAmI, 0, healAmount);
					targets++;
				}

				// Stop if the amount of targets is 6
				//
				if (targets >= 6)
                {
                    break;
                }
			}

			// Add dust effect
			for (int i = 0; i < 15; i++)
			{
				int dust = Dust.NewDust(player.position, player.width, player.height, DustID.CrimsonTorch);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].velocity *= 1.5f;
			}
        }

        public override int MaxAnimationFrames => 75;

		private float _rot = 0;
		public override void RingUseAnimation(Player player, int frame)
        {
            SoundEngine.PlaySound(SoundID.Item15);

			for (int i = 0; i < 2; i++)
			{
				_rot += MathHelper.ToRadians(67.5f);
				_rot = _rot % MathHelper.ToRadians(360);

				Vector2 dustPos = player.Center + new Vector2(35, 0).RotatedBy(_rot);
				Vector2 diff = player.Center - dustPos;
				diff.Normalize();

				int dustType = DustID.CrimsonTorch;
				Dust.NewDustPerfect(dustPos, dustType, diff * 2, Scale: 1.75f).noGravity = true;

				dustType = DustID.Blood;
				Dust.NewDustPerfect(dustPos, dustType, diff * 2, Scale: 1f).noGravity = true;
			}
			_rot += MathHelper.ToRadians(1);
		}
    }
}
