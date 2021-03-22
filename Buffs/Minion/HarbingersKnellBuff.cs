using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Buffs.Minion
{
    public class HarbingersKnellBuff : ModBuff
    {

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Omen");
            Description.SetDefault("A galactic spawn that will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            SupernovaPlayer modPlayer = (SupernovaPlayer)player.GetModPlayer(mod, "SupernovaPlayer");

            if (player.ownedProjectileCounts[mod.ProjectileType("HarbingersKnellProjectile")] > 0)
            {
                modPlayer.minionHairbringersKnell = true;
            }
            if (!modPlayer.minionHairbringersKnell)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
                player.buffTime[buffIndex] = 18000;
        }
    }
}