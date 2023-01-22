using Supernova.Api;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Bosses.StoneMantaRay
{
    public class StoneGlove : ModShotgun
    {
        public override float SpreadAngle => 4;

        public override int GetShotAmount() => 3;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Surgestone Glove");
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.crit = 3;
            Item.knockBack = 2f;
            Item.useStyle = 1;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 23;
            Item.useTime = 23;
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.value = 5000;
            Item.rare = 2;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<Global.Projectiles.Thrown.StoneProj>();
            Item.DamageType = DamageClass.Throwing;
        }
    }
}
