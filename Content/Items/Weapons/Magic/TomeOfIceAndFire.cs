using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Supernova.Api;
using Terraria.GameContent.Creative;

namespace Supernova.Content.Items.Weapons.Magic
{
    public class TomeOfIceAndFire : ModShotgun
    {
        public override float SpreadAngle => 8;

        public override int GetShotAmount() => 5;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Tome of Frost and Fire");

            Tooltip.SetDefault("Shoots fire and frostburn");
        }

        int _leftClick = 0;

        public override void SetDefaults()
        {
            Item.damage = 22;  //The damage stat for the Weapon.
            Item.crit = 3;
            Item.knockBack = 2;
            Item.noMelee = true;  //Setting to True allows the weapon sprite to stop doing damage, so only the projectile does the damge
            Item.noUseGraphic = false;
            Item.channel = true;                            //Channel so that you can held the weapon
            Item.rare = ItemRarityID.Orange;   //The color the title of your Weapon when hovering over it ingame
            Item.width = 28;   //The size of the width of the hitbox in pixels.
            Item.height = 30;    //The size of the height of the hitbox in pixels.
            Item.UseSound = SoundID.Item20;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.shootSpeed = 4.25f;
            Item.mana = 9;
            Item.useStyle = 5;   //The way your Weapon will be used, 5 is the Holding Out Used for: Guns, Spellbooks, Drills, Chainsaws, Flails, Spears for example
            Item.value = Item.sellPrice(0, 10, 0, 0);//	How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 3gold)
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Magic;

            if (_leftClick == 0)
            {
                Item.shoot = ModContent.ProjectileType<Projectiles.Magic.FrostFlame>();
                _leftClick = 1;
            }
            else
            {
                Item.shoot = ProjectileID.Flames;
                _leftClick = 0;
            }
        }

        public override bool CanUseItem(Player player)
        {

            if (_leftClick == 0)
            {
                Item.shoot = ModContent.ProjectileType<Projectiles.Magic.FrostFlame>();
                _leftClick = 1;
            }
            else
            {
                Item.shoot = ProjectileID.Flames;
                _leftClick = 0;
            }
            return base.CanUseItem(player);
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            base.OnHitNPC(player, target, damage, knockBack, crit);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Book);
            recipe.AddIngredient(ModContent.ItemType<BlazeBolt>());
            recipe.AddIngredient(ModContent.ItemType<IceBolt>());
            recipe.AddIngredient(ItemID.Bone, 32);
            recipe.AddIngredient(ItemID.Book);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }

    }
}
