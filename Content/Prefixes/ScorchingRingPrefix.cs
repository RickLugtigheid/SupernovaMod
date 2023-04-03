using Terraria.ID;

namespace SupernovaMod.Content.Prefixes
{
    internal class ScorchingRingPrefix : RingPrefix
    {
		public override string Name => "Scorching";

		public ScorchingRingPrefix() : base(1.05f, tier: -2) { }
	}
	internal class FieryRingPrefix : RingPrefix
	{
		public override string Name => "Fiery";

		public FieryRingPrefix() : base(1.04f, tier: -1) { }
	}
	internal class HeatedRingPrefix : RingPrefix
	{
		public override string Name => "Heated";

		public HeatedRingPrefix() : base(1.02f, tier: -1) { }
	}
}
