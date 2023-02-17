using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Npcs.Bosses.HarbingerOfAnnihilation
{
    public class HarbingersPick : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Harbingers Pick");
        }

        public override void SetDefaults()
        {
            Item.damage = 6; // Base Damage of the Weapon
            Item.width = 24; // Hitbox Width
            Item.height = 24; // Hitbox Height

            Item.useTime = 22; // Speed before reuse
            Item.useAnimation = 22; // Animation Speed
            Item.useStyle = ItemUseStyleID.Swing; // 1 = Broadsword 
            Item.knockBack = 1.5f; // Weapon Knockbase: Higher means greater "launch" distance
            Item.value = 5500; // 10 | 00 | 00 | 00 : Platinum | Gold | Silver | Bronze
            Item.rare = ItemRarityID.Green; // Item Tier
            Item.UseSound = SoundID.Item1; // Sound effect of item on use 
            Item.autoReuse = true; // Do you want to torture people with clicking? Set to false

            Item.pick = 55; // Pick Power - Higher Value = Better
            Item.tileBoost += 6;
        }
    }
}
