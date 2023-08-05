using Microsoft.Xna.Framework;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class HellfireRifle : SupernovaGunItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Hellfire Rifle");
            // Tooltip.SetDefault("18% chance not to consume ammo\nTurns Wooden bullets into Molten Bullets");
        }

        public override Vector2? HoldoutOffset() => new Vector2(-2, -2);

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 17;
            Item.width = 72;
            Item.crit = 1;
            Item.height = 26;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.noMelee = true; //so the item's animation doesn't do damage
            Item.knockBack = 2.4f;
            Item.value = Item.buyPrice(0, 15, 50, 0);
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item11;
            Item.shootSpeed = 11f;
            Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;

            Item.scale = .8f;

            Gun.spread = 5;
            Gun.recoil = .3f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<WoodenRifle>());
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.FirearmManual>(), 2);
            recipe.AddIngredient(ItemID.HellstoneBar, 17);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override void OnConsumeAmmo(Item ammo, Player player)
        {
            // An 18% chance not to consume ammo
            if (Main.rand.NextFloat() >= .18f)
            {
                base.OnConsumeAmmo(ammo, player);
            }
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Convert wooden bullets to molten bullets
            if (type == ModContent.ProjectileType<Projectiles.Ranged.Bullets.WoodenBullet>())
            {
                type = ModContent.ProjectileType<Projectiles.Ranged.Bullets.MoltenBullet>();
            }
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
    }
}