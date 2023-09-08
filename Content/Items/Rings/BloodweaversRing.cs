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

            /* Tooltip.SetDefault("When the 'Ring Ability button' is pressed" +
                "\n You will drain life from the 10 nearest enemies."); */
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 6, 0, 0);
            Item.accessory = true;

			damage = 24;
		}
		public override int BaseCooldown => 60 * 140;
		public override void RingActivate(Player player, float ringPowerMulti)
        {
			SoundEngine.PlaySound(SoundID.Item14, player.Center);

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
                    int healAmount = (int)(damage / 3);
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
		public override void RingUseAnimation(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item15);

			/*Vector2 dustPos = player.Center + new Vector2(30, 0).RotatedByRandom(MathHelper.ToRadians(360));
            Vector2 diff = player.Center - dustPos;
            diff.Normalize();

			Dust.NewDustPerfect(dustPos, DustID.CrimsonTorch, diff * 3, Scale: 1.5f).noGravity = true;
			Dust.NewDustPerfect(dustPos, DustID.Blood, diff * 2).noGravity = true;*/

			/*Vector2 dustPos = player.Center + new Vector2(15, 0).RotatedBy(_rot);

			for (int i = 0; i < 2; i++)
			{
				_rot += MathHelper.ToRadians(67.5f); // 45
				_rot = _rot % MathHelper.ToRadians(360);

				dustPos = dustPos.RotatedBy(_rot);
				Vector2 diff = dustPos - player.Center;
				diff.Normalize();

				int dustType = DustID.CrimsonTorch;
				Dust.NewDustPerfect(dustPos, dustType, diff, Scale: 1.75f).noGravity = true;

				dustType = DustID.Blood;
				Dust.NewDustPerfect(dustPos, dustType, diff, Scale: 1.5f).noGravity = true;
			}
			_rot += MathHelper.ToRadians(1);*/

			// Spawn gem dust on the player
			//
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
