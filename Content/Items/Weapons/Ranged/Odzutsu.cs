using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using SupernovaMod.Api;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class Odzutsu : SupernovaGunItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 30;
            Item.width = 64;
            Item.crit = 2;
            Item.height = 32;
            Item.useAnimation = 67;
            Item.useTime = 67;
            Item.knockBack = 5f;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 30);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item36;
            Item.shootSpeed = 10;
            Item.scale = .8f;

            Gun.recoil = 1.3f;
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
            type = ProjectileID.CannonballFriendly;
			base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
		}

	}
}