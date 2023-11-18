using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class WoodenRifle : SupernovaGunItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Wooden Rifle");
            // Tooltip.SetDefault("18% chance not to consume ammo");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 9;
            Item.width = 50;
            Item.crit = 1;
            Item.height = 28;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.knockBack = 1.2f;
            Item.value = Item.buyPrice(0, 10, 50, 0);
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item11;
            Item.shootSpeed = 6;

            Item.scale = .8f;

            Gun.spread = 5;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<WoodGun>());
            recipe.AddIngredient(ItemID.IronBar, 7);
            //recipe.anyIronBar = true;
            recipe.acceptedGroups = new() { RecipeGroupID.IronBar };
            recipe.AddIngredient(ModContent.ItemType<Materials.FirearmManual>());
            recipe.AddIngredient(ItemID.Wood, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override void OnConsumeAmmo(Item ammo, Player player)
        {
            // An 18% chance not to consume ammo
            if (Main.rand.NextFloat() >= .12f)
            {
                base.OnConsumeAmmo(ammo, player);
            }
        }
    }
}