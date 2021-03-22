using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace Supernova.Items.Misc
{
    public class HorridChunk : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Horrid Chunk");
            Tooltip.SetDefault("dropped by Terror Bat and can only be used at night" +
                "\nSummons the Flying Terror");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 1, 34, 16);
            item.rare = Rarity.Orange;
            item.useAnimation = 25;
            item.useTime = 30;
            item.consumable = true;

            item.useStyle = ItemUseStyleID.HoldingUp; // Holds up like a summon item.
        }

        public override bool CanUseItem(Player player)
        {
            // Does NPC already Exist?
            bool alreadySpawned = NPC.AnyNPCs(mod.NPCType("FlyingTerror"));
            return !alreadySpawned;
        }

        public override bool UseItem(Player player)
        {
            // Check if not daytime
            if (Main.dayTime) return false;

            // Than summon the Flying Terror
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("FlyingTerror")); // Spawn the boss within a range of the player. 
            Main.PlaySound(SoundID.Roar, player.position, 0); // Play spawn sound
            return true;
        }
    }
}
