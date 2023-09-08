using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria.WorldBuilding;
using Terraria.Audio;
using Terraria.DataStructures;
using SupernovaMod.Content.Projectiles.Melee.Spears;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class ConquerorsBladeStaff : ModItem
    {
		public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			ItemID.Sets.SkipsInitialUseSound[Item.type] = true; // This skips use animation-tied sound playback, so that we're able to make it be tied to use time instead in the UseItem() hook.
			ItemID.Sets.Spears[Item.type] = true; // This allows the game to recognize our new item as a spear.
		}

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.crit = 5;
            Item.width = 52;
            Item.height = 62;
            Item.useTime = 34;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;

			Item.noUseGraphic = true;
			Item.noMelee = true;

			// Projectile Properties
			//
			Item.shootSpeed = 3.7f; // The speed of the projectile measured in pixels per frame.
			Item.shoot = ModContent.ProjectileType<ConquerorsBladeStaffProj>(); // The projectile that is fired from this weapon
		}

		public override bool CanUseItem(Player player)
		{
			// Ensures no more than one spear can be thrown out, use this when using autoReuse
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool? UseItem(Player player)
		{
			// Because we're skipping sound playback on use animation start, we have to play it ourselves whenever the item is actually used.
            //
			if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}

			return null;
		}
		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float swingOffset = Utils.NextFloat(Main.rand, 0.5f, 1f) * base.Item.shootSpeed * 1.6f * (float)player.direction;
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, swingOffset, player.altFunctionUse);
			return false;
		}
	}
}
