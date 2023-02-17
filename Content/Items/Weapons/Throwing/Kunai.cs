using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Throwing
{
    public class Kunai : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;

            DisplayName.SetDefault("Kunai");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999; // Makes it so the weapon stacks.
            Item.damage = 14;
            Item.crit = 3;
            Item.knockBack = 1f;
            Item.useStyle = 1;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 7;
            Item.useTime = 7;
            Item.width = 30;
            Item.height = 30;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.consumable = true; // Makes it so one is taken from stack after use.
            Item.value = Item.buyPrice(0, 0, 0, 30);
            Item.rare = ItemRarityID.Green;
            Item.shootSpeed = 19f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.KunaiProj>();

            Item.DamageType = DamageClass.Throwing;
        }
    }
}
