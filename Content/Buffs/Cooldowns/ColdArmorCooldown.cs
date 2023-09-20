using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.Cooldowns
{
    public class ColdArmorCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

		public override void Update(Player player, ref int buffIndex)
		{
            if (Main.rand.NextBool(3))
            {
				Vector2 dustPos = player.Center + new Vector2(23, 0).RotatedByRandom(MathHelper.ToRadians(360));
				Vector2 diff = player.Center - dustPos;
				diff.Normalize();

				Dust.NewDustPerfect(dustPos, DustID.IceRod, diff * 2).noGravity = true;
				base.Update(player, ref buffIndex);
			}
		}
	}
}