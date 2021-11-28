using Terraria;
using Terraria.ModLoader;

namespace Supernova.Items.Armor.PreHardmode.Carnage
{
    [AutoloadEquip(EquipType.Head)]
    public class CarnageHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Helmet");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 7, 0, 0);
            item.rare = Rarity.Green;
            item.defense = 6; // The Defence value for this piece of armour.
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("CarnageBreastplate") && legs.type == mod.ItemType("CarnageBoots");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "When you get hit may Rage Giving you +10% crit \nReduces damage taken by 5%";
            player.AddBuff(ModContent.BuffType<Buffs.Minion.CarnageOrbBuff>(), 10);

            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Minions.CarnageOrb>()] < 1)
                Projectile.NewProjectile(player.position, Microsoft.Xna.Framework.Vector2.Zero, ModContent.ProjectileType<Projectiles.Minions.CarnageOrb>(), item.damage, 1, player.whoAmI);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BloodShards"), 23);
            recipe.AddIngredient(mod.GetItem("BoneFragment"), 12);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
