using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Terraria.Audio;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class StarNight : ModItem
    {
        private const int STAR_AMMOUNT = 4;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Starry Night");
            // Tooltip.SetDefault("This weapon charges up when you shoot.\nWhen fully charged it will shoot 4 deadly stars at your enemies");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
        int _shots = 0;
        bool _playAnimation = false;

        public override void SetDefaults()
        {
            Item.crit = 1;
            Item.width = 16;
            Item.height = 24;
            SetDefaultsArrow();

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            Item.value = Item.buyPrice(0, 2, 77, 0); // Another way to handle value of item.
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item5; // Sound for Bows
            Item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.DamageType = DamageClass.Ranged;
        }

        private void SetDefaultsArrow()
        {
			Item.autoReuse = false;
			Item.damage = 15;
			Item.useAnimation = 24;
			Item.useTime = 24;
			Item.UseSound = SoundID.Item5; // Sound for Bows
			Item.shootSpeed = 6;
		}
		private void SetDefaultsStarBarrage()
        {
			Item.autoReuse = true;
			Item.shootSpeed = 12;
			Item.useAnimation = 4;
			Item.useTime = 4;
			//Item.UseSound = SoundID.Item29;
			Item.UseSound = SoundID.Item9;
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);

            //if (type == ProjectileID.WoodenArrowFriendly) // or ProjectileID.WoodenArrowFriendly
            {
                if (_shots > 6)
                {
					type = ProjectileID.StarCannonStar;
					SetDefaultsStarBarrage();

                    if (_shots >= (6 + STAR_AMMOUNT))
                    {
                        _shots = 0;
                    }
                }
                else
                {
                    SetDefaultsArrow();

					if (_shots > 5)
                    {
                        //_shots = 0;
						_playAnimation = true;
                    }
                }
            }
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (_playAnimation == true)
			{
				for (int i = 0; i < 50; i++)
				{
					int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Enchanted_Pink, Scale: 1.4f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].velocity *= 2f;
				}
                SoundEngine.PlaySound(SoundID.Item4);
				_playAnimation = false;
				SetDefaultsStarBarrage(); // Next shot will be a star
			}
			_shots++;
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
	}
}
