using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Npcs.Bosses.HarbingerOfAnnihilation
{
    public class HarbingersPick : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harbingers Pick");
        }

        public override void SetDefaults()
        {

            item.damage = 6; // Base Damage of the Weapon
            item.width = 24; // Hitbox Width
            item.height = 24; // Hitbox Height

            item.useTime = 22; // Speed before reuse
            item.useAnimation = 22; // Animation Speed
            item.useStyle = ItemUseStyleID.SwingThrow; // 1 = Broadsword 
            item.knockBack = 1.5f; // Weapon Knockbase: Higher means greater "launch" distance
            item.value = 5500; // 10 | 00 | 00 | 00 : Platinum | Gold | Silver | Bronze
            item.rare = ItemRarityID.Green; // Item Tier
            item.UseSound = SoundID.Item1; // Sound effect of item on use 
            item.autoReuse = true; // Do you want to torture people with clicking? Set to false

            item.pick = 55; // Pick Power - Higher Value = Better
            item.tileBoost += 6;
        }
    }
}
