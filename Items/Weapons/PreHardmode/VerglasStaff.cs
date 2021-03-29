using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class VerglasStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Staff");
            Tooltip.SetDefault("Summons a verglas flake to fight for you");
        }

        public override void SetDefaults()
        {
            item.damage = 12;
            item.summon = true;
            item.mana = 42;
            item.width = 20;
            item.height = 20;

            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 0.5f;
            item.value = Item.buyPrice(0, 7, 80, 0);
            item.rare = Rarity.Orange;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("VerglasFlakeProjectile");
            item.shootSpeed = 1f;
            item.buffType = mod.BuffType("VerglasFlakeBuff");
            item.buffTime = 3600;
        }

        public override bool AltFunctionUse(Player player) => true;
        
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) => player.altFunctionUse != 2;

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
                player.MinionNPCTargetAim();
            return base.UseItem(player);
        }
    }
}
