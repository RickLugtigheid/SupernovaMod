using SupernovaMod.Content.Items.Rings.BaseRings;
using Terraria.ID;

namespace SupernovaMod.Content.Prefixes
{
	internal class MenacingRingPrefix : RingPrefix
	{
		public override string Name => "Menacing";
		public override RingType RingCategory => RingType.Projectile;

		public MenacingRingPrefix() : base(projectileDamageMulti: 1.04f, tier: 2) { }
	}
	internal class AngryRingPrefix : RingPrefix
	{
		public override string Name => "Angry";
		public override RingType RingCategory => RingType.Projectile;

		public AngryRingPrefix() : base(projectileDamageMulti: 1.03f, tier: 1) { }
	}
	internal class SpikedRingPrefix : RingPrefix
	{
		public override string Name => "Spiked";
		public override RingType RingCategory => RingType.Projectile;

		public SpikedRingPrefix() : base(projectileDamageMulti: 1.02f, tier: 1) { }
	}
}
