using Terraria;
using Terraria.ModLoader;
using Supernova.Common.Players;

namespace Supernova.Content.Buffs.Minion
{
    public class VerglasFlakeBuff : ModBuff
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Flake");
            Description.SetDefault("A verglas flake that will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            AccessoryPlayer modPlayer = player.GetModPlayer<AccessoryPlayer>();

            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.VerglasFlakeMinion>()] > 0)
            {
                modPlayer.hasMinionVerglasFlake = true;
            }
            if (!modPlayer.hasMinionVerglasFlake)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}