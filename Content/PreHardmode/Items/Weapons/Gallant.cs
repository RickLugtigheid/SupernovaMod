using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class Gallant : ModItem
    {
        int _shots;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gallant");
            Tooltip.SetDefault("Can shoot 6 bullets before having to cooldown");
        }

        public override Vector2? HoldoutOffset() => new Vector2(-2, 0);

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.width = 40;
            Item.crit = 3;
            Item.height = 20;
            Item.useStyle = 5;
            Item.noMelee = true; //so the item's animation doesn't do damage
            Item.knockBack = 0.7f;
            Item.value = Item.buyPrice(0, 4, 30, 0);
            Item.autoReuse = false;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item38;
            Item.shoot = 10; //idk why but all the guns in the vanilla source have this
            Item.shootSpeed = 9f;
            Item.useAmmo = AmmoID.Bullet;

            Item.useTime = 3;
            Item.useAnimation = 3;

            Item.scale = .8f;

            Item.DamageType = DamageClass.Ranged;
        }
        public override bool CanUseItem(Player player)
		{
            // Check if our Gallant is cooling down
            if (player.HasBuff(ModContent.BuffType<Global.Buffs.GallantCooldown>()))
                return false;

            // After 6 shots the player will get the GallantCooldown debuff
            _shots++;
            if (_shots >= 6)
			{
                player.AddBuff(ModContent.BuffType<Global.Buffs.GallantCooldown>(), Item.useTime * 60); // The use time will be the ammount of seconds needed for cooldown
                _shots = 0;
			}
            return base.CanUseItem(player);
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