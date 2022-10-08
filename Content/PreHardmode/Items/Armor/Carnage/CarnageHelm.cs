using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Armor.Carnage
{
    [AutoloadEquip(EquipType.Head)]
    public class CarnageHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Carnage Helmet");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 7, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 6; // The Defence value for this piece of armour.
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<CarnageBreastplate>() && legs.type == ModContent.ItemType<CarnageBoots>();

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "When you get hit may Rage Giving you +10% crit \nReduces damage taken by 5%";
            player.AddBuff(ModContent.BuffType<Global.Buffs.Minion.CarnageOrbBuff>(), 10);

            if (player.ownedProjectileCounts[ModContent.ProjectileType<Global.Minions.CarnageOrb>()] < 1)
			{
                Projectile.NewProjectile(player.GetSource_Misc("SetBonus_CarnageArmor"), player.position, Microsoft.Xna.Framework.Vector2.Zero, ModContent.ProjectileType<Global.Minions.CarnageOrb>(), 18, 1, player.whoAmI);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.BloodShards>(), 23);
            recipe.AddIngredient(ModContent.ItemType<Materials.BoneFragment>(), 12);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
