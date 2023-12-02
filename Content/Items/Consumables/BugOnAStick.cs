using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.GameContent.Creative;
using SupernovaMod.Api;

namespace SupernovaMod.Content.Items.Consumables
{
    public class BugOnAStick : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = BuyPrice.RarityWhite;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 25;
            Item.useTime = 30;
            Item.consumable = true;

            Item.useStyle = ItemUseStyleID.HoldUp; // Holds up like a summon item.
        }

        public override bool CanUseItem(Player player)
        {
            // Does NPC already Exist?
            bool alreadySpawned = NPC.AnyNPCs(ModContent.NPCType<Npcs.FlyingTerror.FlyingTerror>());
            return !alreadySpawned;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            // Check if not daytime
            if (Main.dayTime) return false;

            // Than summon the Flying Terror
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Npcs.FlyingTerror.FlyingTerror>()); // Spawn the boss within a range of the player. 
            SoundEngine.PlaySound(SoundID.Roar, player.position); // Play spawn sound
            return true;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LadyBug);
			recipe.acceptedGroups = new() { RecipeGroupID.Bugs, RecipeGroupID.Wood };
			recipe.AddIngredient(ItemID.Wood);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}
