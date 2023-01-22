using Microsoft.Xna.Framework;
using Supernova.Content.PreHardmode.Items.Weapons.BaseWeapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons.Ranged
{
    public class Gallant : SupernovaGunItem
    {
        int _shots = 0;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Gallant");
            Tooltip.SetDefault("Can shoot 6 bullets before having to cooldown.");
        }

        public override Vector2? HoldoutOffset() => new Vector2(-2, 0);

        public override void SetDefaults()
        {
			base.SetDefaults();

			Item.damage = 10;
            Item.width = 40;
            Item.crit = 3;
            Item.height = 20;
            Item.noMelee = true; //so the item's animation doesn't do damage
            Item.knockBack = 0.7f;
            Item.value = Item.buyPrice(0, 4, 30, 0);
            Item.autoReuse = false;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item38;
            Item.shootSpeed = 8;
            Item.useAmmo = AmmoID.Bullet;

            Item.useAnimation = 4;
            Item.useTime = 2;
            Item.reuseDelay = 2;

            Item.scale = .8f;
        }
        public override bool CanUseItem(Player player)
        {
            // Check if our Gallant is cooling down
            if (player.HasBuff(ModContent.BuffType<Global.Buffs.GallantCooldown>()))
                return false;

            // After 6 shots the player will get the GallantCooldown debuff
            _shots++;
            if (_shots >= 3)
            {
                player.AddBuff(ModContent.BuffType<Global.Buffs.GallantCooldown>(), Item.useTime * 30); // The use time will be the ammount of seconds needed for cooldown
                _shots = 0;
            }
            return base.CanUseItem(player);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Add random spread to our projectile
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(2));

            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldBar, 7);
            recipe.AddIngredient(ItemID.FlintlockPistol);
            recipe.AddIngredient(ModContent.ItemType<Materials.FirearmManual>());
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumBar, 7);
            recipe.AddIngredient(ItemID.FlintlockPistol);
            recipe.AddIngredient(ModContent.ItemType<Materials.FirearmManual>());
            recipe.Register();
        }
    }
}