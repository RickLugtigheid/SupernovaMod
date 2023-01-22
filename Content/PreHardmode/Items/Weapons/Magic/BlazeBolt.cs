using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Supernova.Api;
using Terraria.GameContent.Creative;

namespace Supernova.Content.PreHardmode.Items.Weapons.Magic
{
    public class BlazeBolt : ModShotgun
    {
        public override float SpreadAngle => 8;

        public override int GetShotAmount() => 5;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Blaze Bolt");

            Tooltip.SetDefault("Shoots fire to burn your enemies");
        }
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.crit = 3;
            Item.knockBack = 0.5f;
            Item.noMelee = true;
            Item.noUseGraphic = false;
            Item.channel = true;
            Item.rare = ItemRarityID.Green;
            Item.width = 28;     // The size of the width of the hitbox in pixels.
            Item.height = 30;    // The size of the height of the hitbox in pixels.
            Item.UseSound = SoundID.Item20;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.shootSpeed = 3.25f;
            Item.mana = 7;
            Item.useStyle = 5;   //The way your Weapon will be used, 5 is the Holding Out Used for: Guns, Spellbooks, Drills, Chainsaws, Flails, Spears for example
            Item.value = Item.sellPrice(0, 3, 35, 64);
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Flames;

            Item.DamageType = DamageClass.Magic;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Torch, 99);
            recipe.AddIngredient(ItemID.Fireblossom);
            recipe.AddIngredient(ItemID.LavaBucket, 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
