using Terraria.ID;

namespace SupernovaMod.Content.Prefixes
{
    internal class FrigidRingPrefix : RingPrefix
    {
		public override string Name => "Frigid";

		public FrigidRingPrefix() : base(.95f, tier: 2) { }
	}
	internal class CooledRingPrefix : RingPrefix
	{
		public override string Name => "Cooled";

		public CooledRingPrefix() : base(.96f, tier: 1) { }
	}
	internal class ChillyRingPrefix : RingPrefix
	{
		public override string Name => "Chilly";

		public ChillyRingPrefix() : base(.98f, tier: 1) { }
	}
}
