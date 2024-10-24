using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class Magnitrix : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, 0);
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.crit = 2;
			Item.knockBack = 5;
			Item.width = 40;
			Item.height = 74;
			Item.useAnimation = 32;
			Item.useTime = 32;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.value = Item.buyPrice(1);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item5;
			Item.useAmmo = AmmoID.Arrow;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 14;

			Item.DamageType = DamageClass.Ranged;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			float rot = position.ToRotation();
            Projectile.NewProjectile(source, position + (Vector2.One * 5).RotatedBy(rot), velocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position + (-Vector2.One * 5).RotatedBy(rot), velocity, type, damage, knockback, player.whoAmI);
			return false;
        }
    }
}
