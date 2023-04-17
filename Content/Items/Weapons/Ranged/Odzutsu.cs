using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class Odzutsu : SupernovaGunItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Odzutsu");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 16;
            Item.width = 30;
            Item.height = 18;
            Item.crit = 1;
            Item.useTime = 56;
            Item.useAnimation = 56;
            Item.knockBack = 1.7f;
			Item.value = BuyPrice.SoldRarityGreen;
			Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item41;
            Item.shootSpeed = 5.5f;

            Gun.spread = 1;
            Gun.recoil = 1.25f;
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            
            // Always shoot cannon balls
            type = ProjectileID.CannonballFriendly;
		}
	}
}