using Microsoft.Xna.Framework;
using Supernova.Api;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons.Magic
{
    public class StaffOfThorns : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Staff of Thorns");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.crit = 4;
            Item.width = 28;
            Item.height = 34;

            Item.useAnimation = 12;
            Item.useTime = 3;
            Item.reuseDelay = 30;

            Item.useStyle = 5;        //this is how the item is holded
            Item.noMelee = true;
            Item.knockBack = 1.7f;
            Item.value = Item.buyPrice(0, 5, 77, 0); // Another way to handle value of item.
            Item.rare = ItemRarityID.Green;
            Item.mana = 4;             //mana use
            Item.UseSound = SoundID.Item21;            //this is the sound when you use the item
            Item.autoReuse = true;
            //Item.shoot = ModContent.ProjectileType<Global.Projectiles.ThornBal>();
            Item.shoot = ProjectileID.SeedlerThorn;
            Item.shootSpeed = 5;    //projectile speed when shoot

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