using SupernovaMod.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.Summon
{
    public class HarbingersKnellBuff : ModBuff
    {

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

		public override void Update(Player player, ref int buffIndex)
		{
			AccessoryPlayer modPlayer = player.GetModPlayer<AccessoryPlayer>();

			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.OmenMinion>()] > 0)
			{
				modPlayer.hasMinionHairbringersKnell = true;
			}
			if (!modPlayer.hasMinionHairbringersKnell)
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