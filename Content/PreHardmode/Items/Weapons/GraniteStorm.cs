using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Supernova.Api;
using Terraria.GameContent.Creative;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
	public class GraniteStorm : ModShotgun
    {
		public override float SpreadAngle => 6 * 4;
		public override int MinShots => 3;
		public override int MaxShots => 5;

		public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Granite Storm");
            Tooltip.SetDefault("Shoots chunks of granite at your enemies");
        }
        public override void SetDefaults()
        {
            Item.damage = 11;  //The damage stat for the Weapon.
            Item.crit = 4;
            Item.noMelee = true;  //Setting to True allows the weapon sprite to stop doing damage, so only the projectile does the damge
            Item.noUseGraphic = false;
            Item.channel = true;                            //Channel so that you can held the weapon
            Item.rare = ItemRarityID.Green;   //The color the title of your Weapon when hovering over it ingame
            Item.width = 28;   //The size of the width of the hitbox in pixels.
            Item.height = 30;    //The size of the height of the hitbox in pixels.
            Item.UseSound = SoundID.Item17;  //The sound played when using your Weapon
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.shootSpeed = 9;
            Item.mana = 5;
            Item.useStyle = 5;   //The way your Weapon will be used, 5 is the Holding Out Used for: Guns, Spellbooks, Drills, Chainsaws, Flails, Spears for example
            Item.value = Item.sellPrice(0, 4, 80, 64);//	How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 3gold)
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Global.Projectiles.GraniteProj>();
            Item.DamageType = DamageClass.Magic;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GraniteBlock, 50);
            recipe.AddIngredient(ItemID.ManaRegenerationPotion);
            recipe.AddIngredient(ItemID.Silk, 2);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }

    }
}
