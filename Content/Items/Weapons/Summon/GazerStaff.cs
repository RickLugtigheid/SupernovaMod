using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Summon
{
    public class GazerStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

			DisplayName.SetDefault("Gazer Staff");
            Tooltip.SetDefault("Summons a Gazer to fight for you");
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.crit = 1;
            Item.mana = 10;
            Item.width = 20;
            Item.height = 20;

            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0.5f;
            Item.value = BuyPrice.RarityBlue;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<Projectiles.Summon.GazerMinion>();
            Item.shootSpeed = 1f;
            Item.buffType = ModContent.BuffType<Buffs.Summon.GazerMinionBuff>();
            Item.buffTime = 3600;
            Item.DamageType = DamageClass.Summon;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => player.altFunctionUse != 2;
    }
}
