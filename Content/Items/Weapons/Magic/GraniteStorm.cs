using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
    public class GraniteStorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Granite Storm");
            Tooltip.SetDefault("Shoots chunks of granite at your enemies");
        }

        public override void SetDefaults()
        {
            Item.damage = 13;  //The damage stat for the Weapon.
            Item.crit = 1;
            Item.knockBack = 6;
            Item.noMelee = true;  //Setting to True allows the weapon sprite to stop doing damage, so only the projectile does the damge
            Item.noUseGraphic = false;
            Item.channel = true;                            //Channel so that you can held the weapon
            Item.rare = ItemRarityID.Green;   //The color the title of your Weapon when hovering over it ingame
            Item.width = 28;   //The size of the width of the hitbox in pixels.
            Item.height = 30;    //The size of the height of the hitbox in pixels.
            Item.UseSound = SoundID.Item17;  //The sound played when using your Weapon
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = .5f;
            Item.mana = 6;
            Item.useStyle = ItemUseStyleID.Shoot;   //The way your Weapon will be used, 5 is the Holding Out Used for: Guns, Spellbooks, Drills, Chainsaws, Flails, Spears for example
            Item.value = Item.sellPrice(0, 4, 80, 64);//	How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 3gold)
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Magic.GraniteProj>();
            Item.DamageType = DamageClass.Magic;
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float baseSpeed = (float)System.Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
			double baseAngle = System.Math.Atan2(velocity.X, velocity.Y);

			var speed = new Vector2(baseSpeed * (float)System.Math.Sin(baseAngle), baseSpeed * (float)System.Math.Cos(baseAngle));

			for (int i = 0; i < 4; ++i)
			{
                Vector2 projVel = new Vector2(Main.rand.Next(-12, 12), Main.rand.Next(-15, -1));
				Projectile.NewProjectile(source, position.X, position.Y, speed.X, speed.Y, type, damage, knockback, player.whoAmI, projVel.X, projVel.Y);
			}
			return false;
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GraniteBlock, 25);
            recipe.AddIngredient(ItemID.ManaCrystal, 2);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }

    }
}
