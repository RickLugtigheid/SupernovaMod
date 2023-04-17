using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
    public class Tessen : SupernovaWarFanItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Tessen");
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
			Item.crit = 1;
			Item.width = 28;
            Item.height = 34;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = BuyPrice.SoldRarityGreen;
            Item.rare = ItemRarityID.Green;
            Item.mana = 3;
            Item.UseSound = SoundID.DD2_BetsyWindAttack;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.DD2SquireSonicBoom;
            Item.shootSpeed = 10;

            Item.DamageType = DamageClass.Magic;
        }
	}
}