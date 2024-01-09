using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Throwing
{
    public class HarpyMothersFeather : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 46;
            Item.crit = 2;
            Item.knockBack = 4;
			Item.width = 22;
			Item.height = 44;
            Item.useAnimation = 28;
            Item.useTime = 28;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
            Item.value = Item.buyPrice(0, 8);
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 14;
            Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.HarpyMothersFeather>();

            Item.DamageType = DamageClass.Throwing;
        }
	}
}
