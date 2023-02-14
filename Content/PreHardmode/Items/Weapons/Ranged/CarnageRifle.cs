using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Supernova.Content.PreHardmode.Items.Weapons.BaseWeapons;
using Terraria.DataStructures;

namespace Supernova.Content.PreHardmode.Items.Weapons.Ranged
{
    public class CarnageRifle : SupernovaGunItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Carnage Rifle");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, -1.5f);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 41;
            Item.width = 58;
            Item.crit = 4;
            Item.height = 20;
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.knockBack = 6.4f;
            Item.autoReuse = false;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item36;
            Item.shootSpeed = 14;

			Gun.recoil = .8f;
            Gun.useStyle = GunUseStyle.PumpAction;
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
            /*if (type == ProjectileID.Bullet)
            {
                type = ProjectileID.BloodShot;
            }*/

			base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
            Main.projectile[proj].hostile = false;
			Main.projectile[proj].friendly = true;

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.BloodShards>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Materials.BoneFragment>(), 7);
            recipe.AddIngredient(ItemID.Musket);
            recipe.AddIngredient(ModContent.ItemType<Materials.FirearmManual>(), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}