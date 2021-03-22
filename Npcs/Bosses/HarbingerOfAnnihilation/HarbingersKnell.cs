using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Npcs.Bosses.HarbingerOfAnnihilation
{
    public class HarbingersKnell : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harbingers Knell");
        }

        public override void SetDefaults()
        {
            item.damage = 8;
            item.summon = true;
            item.mana = 27;
            item.width = 20;
            item.height = 20;

            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 0.5f;
            item.value = Item.buyPrice(0, 4, 80, 0);
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("HarbingersKnellProjectile");
            item.shootSpeed = 1f;
            item.buffType = mod.BuffType("HarbingersKnellBuff");
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
