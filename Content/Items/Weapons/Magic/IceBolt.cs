using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SupernovaMod.Api;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
    public class IceBolt : ModShotgun
    {
        public override float SpreadAngle => 8;

        public override int GetShotAmount() => 5;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Ice Bolt");

            // Tooltip.SetDefault("Shoots frostburn to burn your enemies with -196º(77kelvin)");
        }
        public override void SetDefaults()
        {
            Item.damage = 22;  //The damage stat for the Weapon.
            Item.knockBack = 2;
            Item.crit = 3;
            Item.noMelee = true;  //Setting to True allows the weapon sprite to stop doing damage, so only the projectile does the damge
            Item.noUseGraphic = false;
            Item.channel = true;                            //Channel so that you can held the weapon
            Item.rare = ItemRarityID.Orange;   //The color the title of your Weapon when hovering over it ingame
			Item.width = 28;
			Item.height = 30;
			Item.UseSound = SoundID.Item20;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.shootSpeed = 4f;
            Item.mana = 5;
            Item.useStyle = ItemUseStyleID.Shoot; 
            Item.value = Item.sellPrice(0, 5, 43, 70);
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Magic.FrostFlame>();
            Item.DamageType = DamageClass.Magic;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IceTorch, 80);
            recipe.AddIngredient(ModContent.ItemType<Materials.Rime>(), 7);
			recipe.AddTile(TileID.Bookcases);
			recipe.Register();
        }
    }
}
