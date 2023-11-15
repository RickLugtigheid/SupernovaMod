using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.GameContent.Creative;
using SupernovaMod.Api;

namespace SupernovaMod.Content.Items.Consumables
{
    public class EerieCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
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
            bool alreadySpawned = NPC.AnyNPCs(ModContent.NPCType<Npcs.HarbingerOfAnnihilation.HarbingerOfAnnihilation>());
            return !alreadySpawned;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            // Check if not in the sky
            if (!player.ZoneSkyHeight) return false;

			// Summon the Harbinger of Annihilation
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Npcs.HarbingerOfAnnihilation.HarbingerOfAnnihilation>()); // Spawn the boss within a range of the player. 
			SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, player.position); // Play item break sound
			SoundEngine.PlaySound(SoundID.Roar, player.position); // Play spawn sound
            return true;
        }
	}
}
