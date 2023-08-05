using Microsoft.Xna.Framework;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class WoodGun : SupernovaGunItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Wooden Handgun");
        }
        public override Vector2? HoldoutOffset() => new Vector2(-1, 0);

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 4;
            Item.width = 30;
            Item.height = 18;
            Item.crit = 3;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.knockBack = 1.7f;
            Item.value = Item.buyPrice(0, 1, 70, 0); // Another way to handle value of item.
            Item.autoReuse = false;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item41;
            Item.shootSpeed = 4;

            Gun.spread = 2;
            Gun.recoil = .6f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 15);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}