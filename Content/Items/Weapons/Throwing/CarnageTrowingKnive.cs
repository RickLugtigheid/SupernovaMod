using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using SupernovaMod.Common.Systems;

namespace SupernovaMod.Content.Items.Weapons.Throwing
{
    public class CarnageTrowingKnive : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;

            // DisplayName.SetDefault("Carnage Trowing Knive");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999; // Makes it so the weapon stacks.
            Item.damage = 11;
            Item.crit = 7;
            Item.knockBack = 3f;
            Item.useStyle = 1;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 8;
            Item.useTime = 8;
            Item.width = 30;
            Item.height = 30;
            Item.consumable = true; // Makes it so one is taken from stack after use.
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.value = 64;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 11f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.CarnageTrowingKniveProj>();

			Item.DamageType = GlobalModifiers.DamageClass_ThrowingRanged;
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.BloodShards>(), 3);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.BoneFragment>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

    }
}
