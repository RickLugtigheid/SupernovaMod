using Microsoft.Xna.Framework;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class Cosmolash : ModItem //SupernovaGunItem
    {
        private const int MAX_USE_TIME = 48;
		private const int MIN_USE_TIME = 8;
		public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
		public override Vector2? HoldoutOffset() => new Vector2(-15, 1);
		public override void SetDefaults()
        {
			// SupernovaGunItem SetDefaults 
            //
			Item.noMelee = true; // So the item's animation doesn't do damage
			Item.useAmmo = AmmoID.Bullet;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.DamageType = DamageClass.Ranged;

			base.SetDefaults();

            Item.damage = 38;
			Item.crit = 2;
			Item.width = 76;
            Item.height = 24;
            Item.useTime = MAX_USE_TIME;
            Item.useAnimation = MAX_USE_TIME;
            Item.knockBack = 1.2f;
            Item.value = Item.buyPrice(0, 14, 0, 0);
            Item.autoReuse = true;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item38;
            Item.shootSpeed = 8;
			Item.channel = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.CosmolashGun>();
		}

		public override void OnConsumeAmmo(Item ammo, Player player)
        {
            // An 12% chance not to consume ammo
			//
            if (Main.rand.NextFloat() >= .12f)
            {
                base.OnConsumeAmmo(ammo, player);
            }
        }

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[base.Item.shoot] <= 0;
		}

		// Token: 0x06007EA1 RID: 32417 RVA: 0x00520ADC File Offset: 0x0051ECDC
		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return !Utils.NextBool(Main.rand, 4) && player.ownedProjectileCounts[base.Item.shoot] > 0;
		}

		// Token: 0x06007EA2 RID: 32418 RVA: 0x00520B04 File Offset: 0x0051ED04
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Ranged.CosmolashGun>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
			return false;
		}
	}
}