using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Players
{
    public class ArmorPlayer : ModPlayer
	{
		public bool isWearingZirconiumArmor = false;
		public bool carnageArmor = false;
		public bool coldArmor = false;

		/* Reset */
		public override void ResetEffects()
		{
			base.ResetEffects();

			isWearingZirconiumArmor = false;
			carnageArmor = false;
			coldArmor = false;
		}

		/// <summary>
		/// This method checks if the player is wearing zirconiumArmor, and if so will inflict more damage and OnFire
		/// as described in the Zirconium armor set bonus tooltip.
		/// <para>This should only be run for weapons that should be effected by the Zirconium armor set bonus.</para>
		/// </summary>
		/// <param name="target"></param>
		/// <param name="modifiers"></param>
		public void ZirconiumArmor_ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			// When wearing zirconiumArmor, increase damage by 5 percent,
			// and inflict OnFire for between 2 -> 6 seconds.
			//
			if (isWearingZirconiumArmor)
			{
				modifiers.FinalDamage *= 1.05f;
				target.AddBuff(BuffID.OnFire, Main.rand.Next(2, 6) * 60);
			}
		}
	}
}
