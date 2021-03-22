using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Npcs.Bosses.HarbingerOfAnnihilation
{
    public class HarbingersSlicer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harbingers Slicer");
        }

        public override void SetDefaults()
        {
            item.thrown = true; // Set this to true if the weapon is throwable.
            item.damage = 10;
            item.crit = 3;
            item.knockBack = 0.5f;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 21;
            item.useTime = 21;
            item.width = 30;
            item.height = 30;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = false;
            item.value = 5000;
            item.rare = ItemRarityID.Green;
            item.shootSpeed = 11f;
            item.shoot = mod.ProjectileType("HarbingersSlicerProj");
        }
    }
}
