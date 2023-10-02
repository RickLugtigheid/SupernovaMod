using SupernovaMod.Content.Projectiles.Melee.Swordstaffs;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class HarbingersSlicer : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
			Item.damage = 18;
            Item.crit = 1;
			Item.knockBack = 7;
			Item.useAnimation = (Item.useTime = 25);
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.channel = true;
			Item.autoReuse = true;
			Item.shootSpeed = 14f;
			Item.shoot = ModContent.ProjectileType<HarbingersSlicerProj>();
			Item.width = 128;
			Item.height = 140;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = SoundID.DD2_SkyDragonsFurySwing;
			Item.value = Item.buyPrice(0, 5, 40, 0);
			Item.rare = ItemRarityID.Orange;
		}
    }
}