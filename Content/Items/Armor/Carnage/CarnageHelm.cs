using SupernovaMod.Api;
using SupernovaMod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Armor.Carnage
{
    [AutoloadEquip(EquipType.Head)]
    public class CarnageHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
			Item.value = BuyPrice.RarityGreen;
			Item.rare = ItemRarityID.Green;
			Item.defense = 6; // The Defence value for this piece of armour.
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<CarnageBreastplate>() && legs.type == ModContent.ItemType<CarnageBoots>();

		public override void UpdateEquip(Player player)
		{
            player.GetDamage(DamageClass.Melee) += .07f;
			player.GetAttackSpeed(DamageClass.Melee) += .07f;
		}

		public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Summons a life draining and projectile blocking orb around you, when the orb hits an enemy you will regen life faster for 2 seconds.";

			player.GetModPlayer<ArmorPlayer>().carnageArmor = true;

			// Summon the Carnage orb
			//
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.CarnageOrb>()] < 1)
            {
                Projectile.NewProjectile(player.GetSource_Misc("SetBonus_CarnageArmor"), player.position, Microsoft.Xna.Framework.Vector2.Zero, ModContent.ProjectileType<Projectiles.Summon.CarnageOrb>(), 12, 8, player.whoAmI);
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
