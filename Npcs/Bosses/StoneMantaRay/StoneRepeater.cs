using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Npcs.Bosses.StoneMantaRay
{
    public class StoneRepeater : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Surgestone Repeater");

        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-13, 0);
        }

        public override void SetDefaults()
        {
            item.damage = 16;
            item.ranged = true;
            item.width = 40;
            item.crit = 0;
            item.height = 20;
            item.useAnimation = 2;
            item.useTime = 2;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 8.4f;
            item.value = 50000;
            item.autoReuse = false;
            item.rare = 2;
            item.UseSound = SoundID.Item38;
            item.shoot = 10; //idk why but all the guns in the vanilla source have this
            item.shootSpeed = 11f;
            item.useAmmo = 1;
            item.ranged = true; // For Ranged Weapon
        }
    }
}