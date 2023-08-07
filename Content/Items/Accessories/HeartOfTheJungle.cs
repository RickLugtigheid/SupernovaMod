using SupernovaMod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class HeartOfTheJungle : ModItem
    {
		public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Heart of the Jungle");
            // Tooltip.SetDefault("Stores up to 60 life energy with an increase of 1 per second.\nGetting hit consumes life energy and heals you for the amount of energy you have / 2.");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 2, 35, 89);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BandofRegeneration);
            recipe.AddIngredient(ItemID.Vine, 2);
            recipe.AddIngredient(ItemID.JungleSpores, 6);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.GetModPlayer<AccessoryPlayer>().hasHeartOfTheJungle = true;
			player.GetModPlayer<ResourcePlayer>().lifeEnergyMax2 = 60;
			player.GetModPlayer<ResourcePlayer>().lifeEnergyRegen += .01f;
		}

        public bool ConsumeEnergy(Player player)
        {
			ResourcePlayer resourcePlayer = player.GetModPlayer<ResourcePlayer>();

            // Check if the player lost life
            //
            int lifeLost = player.statLifeMax - player.statLife;
            if (lifeLost < 1)
            {
                return false;
            }

            int energyUsed = 0;
            while (energyUsed < resourcePlayer.lifeEnergy && (energyUsed / 2) < lifeLost)
            {
                energyUsed++;
            }

            // Check if our energy used will heal the player by at least 1 hp
            //
            if (energyUsed <= 0 || (energyUsed / 2) < 1)
            {
                return false;
            }

			resourcePlayer.lifeEnergy -= energyUsed;
            player.Heal(energyUsed / 2);
            return true;
        }
    }
}
