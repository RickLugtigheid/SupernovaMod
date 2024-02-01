using SupernovaMod.Common.GlobalNPCs;
using Terraria;
using Terraria.ID;

namespace SupernovaMod.Common.GlobalBuff
{
	public class SupernovaGlobalBuff : Terraria.ModLoader.GlobalBuff
	{
		public override bool ReApply(int type, NPC npc, int time, int buffIndex)
		{
			DebuffNPC debuffNPC = npc.GetGlobalNPC<DebuffNPC>();

			switch (type)
			{
				case BuffID.Electrified:
					debuffNPC.electrified = time;
					break;
			}
			return base.ReApply(type, npc, time, buffIndex);
		}
	}
}
