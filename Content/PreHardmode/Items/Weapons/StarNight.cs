using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class StarNight : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Starry Night");
            Tooltip.SetDefault("This weapon charges up when you shoot. \nWhen fully charged it will shoot 6 deadly stars at your enemies");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
        int _shots = 5;
        bool _once = true;

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.autoReuse = false;
            Item.crit = 6;
            Item.width = 16;
            Item.height = 24;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            Item.value = Item.buyPrice(0, 2, 77, 0); // Another way to handle value of item.
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item5; // Sound for Bows
            Item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 8;
            Item.DamageType = DamageClass.Ranged;
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);

            //if (type == ProjectileID.WoodenArrowFriendly) // or ProjectileID.WoodenArrowFriendly
            {
                if (_shots < 6)
                {
                    Item.shootSpeed = 17;
                    Item.autoReuse = true;
                    if (_once == true)
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            int dust = Dust.NewDust(player.position, player.width, player.height, 58);
                            Main.dust[dust].scale = 1.5f;
                            Main.dust[dust].noGravity = true;
                            Main.dust[dust].velocity *= 3f;
                            Main.dust[dust].velocity *= 3f;
                        }
                        _once = false;
                    }
                    Item.useAnimation = 4;
                    Item.useTime = 4;
                    Item.UseSound = SoundID.Item29; // Sound for Bows
                    type = ProjectileID.StarCannonStar;
                }
                else
                {
                    Item.shootSpeed = 8;
                    Item.useAnimation = 35;
                    Item.useTime = 35;
                    Item.UseSound = SoundID.Item5; // Sound for Bows
                    Item.autoReuse = false;
                    if (_shots >= 14)
                    {
                        _shots = 0;
                        _once = true;
                    }
                }
                _shots++;
            }
        }
    }
}
