using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Players
{
	internal class ResourcePlayer : ModPlayer
	{
		public float lifeEnergy = 0;
		public int lifeEnergyMax2 = 0;
		public float lifeEnergyRegen = .01f;

		public override void PreUpdate()
		{
			// Reset our max values
			lifeEnergyMax2 = 0;
			lifeEnergyRegen = .001f;

			base.PreUpdate();
		}

		public override void PostUpdateEquips()
		{
			// Update our life energy value
			//
			if (lifeEnergy < lifeEnergyMax2)
			{
				lifeEnergy += lifeEnergyRegen;
			}
		}
	}
}
