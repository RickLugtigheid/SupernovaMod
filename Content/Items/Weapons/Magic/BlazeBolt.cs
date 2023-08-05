using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SupernovaMod.Api;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
    public class BlazeBolt : ModShotgun
    {
        public override float SpreadAngle => 8;

        public override int GetShotAmount() => 4;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Blaze Bolt");

            // Tooltip.SetDefault("Shoots fire to burn your enemies");
        }
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.crit = 2;
            Item.knockBack = 1;
            Item.noMelee = true;
            Item.noUseGraphic = false;
            Item.channel = true;
            Item.rare = ItemRarityID.Green;
            Item.width = 28;
            Item.height = 30;
            Item.UseSound = SoundID.Item20;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.shootSpeed = 3.5f;
            Item.mana = 4;
            Item.useStyle = ItemUseStyleID.Shoot;   //The way your Weapon will be used, 5 is the Holding Out Used for: Guns, Spellbooks, Drills, Chainsaws, Flails, Spears for example
            Item.value = Item.sellPrice(0, 3, 35, 64);
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Flames;

            Item.DamageType = DamageClass.Magic;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Fireblossom);
            recipe.AddIngredient(ItemID.LavaBucket, 2);
			recipe.AddTile(TileID.Bookcases);
			recipe.Register();
        }
    }
}
