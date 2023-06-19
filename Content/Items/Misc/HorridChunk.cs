using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Misc
{
    public class HorridChunk : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;

            DisplayName.SetDefault("Horrid Chunk");
            Tooltip.SetDefault("Dropped by Terror Bats\nSummons the Flying Terror when used at night");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 1, 34, 16);
            Item.rare = ItemRarityID.Orange;
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
    }
}
