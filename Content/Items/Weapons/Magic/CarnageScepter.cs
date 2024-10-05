using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
    public class CarnageScepter : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Carnage Scepter");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.crit = 1;
            Item.width = 28;
            Item.height = 34;
            Item.useTime = 38;
            Item.useAnimation = 38;
            Item.useStyle = ItemUseStyleID.Shoot;        //this is how the item is holded
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.mana = 6;             //mana use
            Item.UseSound = SoundID.Item21;            //this is the sound when you use the item
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Magic.CarnageScepterProj>();
            Item.shootSpeed = 7.5f;    //projectile speed when shoot

            Item.DamageType = DamageClass.Magic;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.BloodShards>(), 12);
            recipe.AddIngredient(ModContent.ItemType<Materials.BoneFragment>(), 8);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}