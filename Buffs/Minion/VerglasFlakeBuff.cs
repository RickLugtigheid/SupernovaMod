using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Buffs.Minion
{
    public class VerglasFlakeBuff : ModBuff
    {

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Verglas Flake");
            Description.SetDefault("A verglas flake that will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            SupernovaPlayer modPlayer = (SupernovaPlayer)player.GetModPlayer(mod, "SupernovaPlayer");

            if (player.ownedProjectileCounts[mod.ProjectileType("VerglasFlakeProjectile")] > 0)
            {
                modPlayer.minionVerglasFlake = true;
            }
            if (!modPlayer.minionVerglasFlake)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
                player.buffTime[buffIndex] = 18000;
        }
    }
}