using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class PoisonousYoYo : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Poisonous YoYo");
        }

        public override void SetDefaults()
        {
            item.knockBack = 1.8f;
            item.damage = 24;
            item.crit = 4;
            item.useStyle = ItemUseStyleID.HoldingOut; // The style used for YoYos.
            item.width = 24;
            item.height = 24;
            item.noUseGraphic = true; // Doesn't show Item in Hand.
            item.melee = true; // YoYos are a melee item.
            item.noMelee = true; // Don't damage enemies with the hand hitbox.
            item.channel = true; // ???
            item.UseSound = SoundID.Item1;
            item.useAnimation = 25;
            item.useTime = 56;
            item.shoot = mod.ProjectileType("PoisonousYoYo"); // Projectile that is used with this weapon.
            item.shootSpeed = 23f; // How fast the projectile is shot.
            item.value = Item.buyPrice(0, 4, 43, 70); // Another way to handle value of item.
            item.rare = Rarity.Orange;
        }

        public override void AddRecipes() 
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.JungleYoyo);
            recipe.AddIngredient(ItemID.Stinger, 7);
            recipe.AddIngredient(ItemID.JungleSpores, 3);
            recipe.AddIngredient(ItemID.Vine, 4);
            recipe.AddIngredient(ItemID.BeeWax);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
