using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
    public class StaffOfThorns : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Staff of Thorns");
            // Tooltip.SetDefault("Shoots a spread of armor piercing thorns.");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.ArmorPenetration = 8;
            Item.damage = 10;
            Item.crit = 4;
            Item.width = 28;
            Item.height = 34;

            Item.useAnimation = 12;
            Item.useTime = 3;
            Item.reuseDelay = 30;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.7f;
            Item.value = Item.buyPrice(0, 5, 77, 0);
            Item.rare = ItemRarityID.Green;
            Item.mana = 4;
            Item.UseSound = SoundID.Item21;
            Item.autoReuse = true;
            //Item.shoot = ModContent.ProjectileType<Global.Projectiles.ThornBal>();
            Item.shoot = ProjectileID.SeedlerThorn;
            Item.shootSpeed = 5;

            Item.DamageType = DamageClass.Magic;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Add random spread to our projectile
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(12));
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
    }
}