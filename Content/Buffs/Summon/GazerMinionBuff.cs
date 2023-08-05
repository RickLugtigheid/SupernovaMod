using Terraria;
using Terraria.ModLoader;
using SupernovaMod.Common.Players;

namespace SupernovaMod.Content.Buffs.Summon
{
    public class GazerMinionBuff : ModBuff
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gazer");
            // Description.SetDefault("A Gazer that will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.GazerMinion>()] > 0)
            {
				player.buffTime[buffIndex] = 18000;
			}
            else
            {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
        }
    }
}