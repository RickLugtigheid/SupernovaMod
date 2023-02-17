using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace Supernova.Content.Items.Weapons.Magic
{
    public class ShockSurge : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Shock Surge");

            Tooltip.SetDefault("Shoots a bolt of lighting at the target");
        }
        public override void SetDefaults()
        {
            Item.damage = 14;  //The damage stat for the Weapon.
            Item.crit = 3;
            Item.noMelee = true;  //Setting to True allows the weapon sprite to stop doing damage, so only the projectile does the damge
            Item.noUseGraphic = false;
            Item.channel = true;                            //Channel so that you can held the weapon
            Item.rare = 3;   //The color the title of your Weapon when hovering over it ingame
            Item.width = 28;   //The size of the width of the hitbox in pixels.
            Item.height = 30;    //The size of the height of the hitbox in pixels.
            Item.UseSound = SoundID.Item20;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.shootSpeed = 27;
            Item.mana = 12;
            Item.useStyle = ItemUseStyleID.Shoot;   //The way your Weapon will be used, 5 is the Holding Out Used for: Guns, Spellbooks, Drills, Chainsaws, Flails, Spears for example
            Item.value = Item.sellPrice(0, 5, 32, 64);//	How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 3gold)
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Magic.Lightning>();

            Item.DamageType = DamageClass.Magic;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.RainCloud, 40);
            recipe.AddIngredient(ItemID.Cloud, 20);
            recipe.AddIngredient(ItemID.ManaCrystal);
            recipe.AddIngredient(ItemID.Silk, 4);
            recipe.AddIngredient(ItemID.Wood, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
