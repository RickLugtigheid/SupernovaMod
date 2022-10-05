using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Content.PreHardmode.Bosses.HarbingerOfAnnihilation
{
    public class HarbingersSlicer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harbingers Slicer");
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.crit = 3;
            Item.knockBack = 0.5f;
            Item.useStyle = 1;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 21;
            Item.useTime = 21;
            Item.width = 30;
            Item.height = 30;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.value = 5000;
            Item.rare = ItemRarityID.Green;
            Item.shootSpeed = 11f;
            Item.shoot = Mod.Find<ModProjectile>("HarbingersSlicerProj").Type;

            Item.DamageType = DamageClass.Throwing;
        }
    }
}
