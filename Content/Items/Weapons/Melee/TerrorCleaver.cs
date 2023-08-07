using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using SupernovaMod.Api;
using SupernovaMod.Content.Npcs.FlyingTerror;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
	public class TerrorCleaver : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Terror Cleaver");
            // Tooltip.SetDefault("Shoots scythes of terror");
            //Tooltip.SetDefault("A big cleaver as strong as the bones of the Flying Terror");
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.crit = 1;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 46;
            Item.useAnimation = 46;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.5f;
			Item.value = BuyPrice.RarityGreen;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Melee.TerrorScythe>();
            Item.shootSpeed = 6;
            Item.DamageType = DamageClass.Melee;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<TerrorTuft>());
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}
