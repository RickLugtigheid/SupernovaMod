using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Npcs.Bosses.HarbingerOfAnnihilation
{
    public class HarbingersSlicer : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Harbingers Slicer");
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.crit = 3;
            Item.knockBack = 0.5f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 21;
            Item.useTime = 21;
            Item.width = 30;
            Item.height = 30;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.value = 5000;
            Item.rare = ItemRarityID.Green;
            Item.shootSpeed = 11f;
            Item.shoot = ModContent.ProjectileType<HarbingersSlicerProj>();

            Item.DamageType = DamageClass.Throwing;
        }
    }
}
