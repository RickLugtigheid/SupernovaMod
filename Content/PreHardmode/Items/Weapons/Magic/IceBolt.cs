using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Supernova.Api;
using Terraria.GameContent.Creative;

namespace Supernova.Content.PreHardmode.Items.Weapons.Magic
{
    public class IceBolt : ModShotgun
    {
        public override float SpreadAngle => 8;

        public override int GetShotAmount() => 5;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Ice Bolt");

            Tooltip.SetDefault("Shoots frostburn to burn your enemies with -196º(77kelvin)");
        }
        public override void SetDefaults()
        {
            Item.damage = 16;  //The damage stat for the Weapon.
            Item.knockBack = 1;
            Item.crit = 3;
            Item.noMelee = true;  //Setting to True allows the weapon sprite to stop doing damage, so only the projectile does the damge
            Item.noUseGraphic = false;
            Item.channel = true;                            //Channel so that you can held the weapon
            Item.rare = ItemRarityID.Orange;   //The color the title of your Weapon when hovering over it ingame
            Item.width = 28;   //The size of the width of the hitbox in pixels.
            Item.height = 30;    //The size of the height of the hitbox in pixels.
            Item.UseSound = SoundID.Item20;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.shootSpeed = 3.4f;
            Item.mana = 7;
            Item.useStyle = 5;   //The way your Weapon will be used, 5 is the Holding Out Used for: Guns, Spellbooks, Drills, Chainsaws, Flails, Spears for example
            Item.value = Item.sellPrice(0, 5, 43, 70);//	How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 3gold)
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Global.Projectiles.Magic.FrostFlame>();
            Item.DamageType = DamageClass.Magic;
        }

        /*public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2[] speeds = randomSpread(speedX, speedY, 8, 6);
            for (int i = 0; i < 5; ++i)
                Projectile.NewProjectile(position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockBack, player.whoAmI);
            return false;
        }*/
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IceTorch, 80);
            recipe.AddIngredient(ModContent.ItemType<Materials.Rime>(), 7);
            recipe.AddIngredient(ItemID.SnowBlock, 200);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}
