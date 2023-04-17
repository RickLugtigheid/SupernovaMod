using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Common.GlobalNPCs
{
	public class DebuffNPC : GlobalNPC
	{
		public int carnageBleed;

		public override bool InstancePerEntity => true;

		public override GlobalNPC Clone(NPC from, NPC to)
		{
			DebuffNPC myClone = (DebuffNPC)base.Clone(from, to);
			myClone.carnageBleed = carnageBleed;
			return myClone;
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (carnageBleed > 0)
			{
				int baseBleedDamage = 5;
				ApplyDamageOverTime(baseBleedDamage, baseBleedDamage / 5, ref npc.lifeRegen, ref damage);
			}
		}

		public override void PostAI(NPC npc)
		{
			if (carnageBleed > 0)
			{
				carnageBleed--;
			}
		}

		public void ApplyDamageOverTime(int lifeRegenVal, int damageVal, ref int lifeRegen, ref int damage)
		{
			if (lifeRegen > 0)
			{
				lifeRegen = 0;
			}
			lifeRegen -= lifeRegenVal;

			if (damage < damageVal)
			{
				damage = damageVal;
			}
		}
	}
}
