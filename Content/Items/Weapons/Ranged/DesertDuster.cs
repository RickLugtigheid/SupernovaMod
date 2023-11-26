using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class DesertDuster : SupernovaGunItem
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Desert Duster");
            // Tooltip.SetDefault("Uses sand to shoot.");
		}

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 24;
            Item.width = 58;
            Item.crit = 1;
            Item.height = 22;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.knockBack = 1.5f;
            Item.value = Item.buyPrice(0, 11, 50, 0);
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item11;
            Item.shootSpeed = 10;
            Item.useAmmo = AmmoID.Sand;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.FirearmManual>(), 2);
            recipe.AddIngredient(ItemID.FossilOre, 12);
			recipe.AddIngredient(ItemID.Amber, 2);
			recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        private int _ammoItemId;
        public override void OnConsumeAmmo(Item ammo, Player player)
        {
            _ammoItemId = ammo.netID;
            // An 8% chance not to consume ammo
            if (Main.rand.NextFloat() >= .8f)
            {
                base.OnConsumeAmmo(ammo, player);
            }
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Add random spread to our projectile
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));

            // Get the correct projectile to shoot
            type = GetProjectileForAmmo();

            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        private int GetProjectileForAmmo()
        {
            switch (_ammoItemId)
            {
                case ItemID.CrimsandBlock:
                    return ModContent.ProjectileType<Projectiles.Ranged.Bullets.CrimsandBullet>();
                case ItemID.EbonsandBlock:
                    return ModContent.ProjectileType<Projectiles.Ranged.Bullets.EbonsandBullet>();
                case ItemID.PearlsandBlock:
                    return ModContent.ProjectileType<Projectiles.Ranged.Bullets.PearlsandBullet>();
                default: // Default & ItemID.SandBlock
                    return ModContent.ProjectileType<Projectiles.Ranged.Bullets.SandBullet>();
            }
        }
    }
}