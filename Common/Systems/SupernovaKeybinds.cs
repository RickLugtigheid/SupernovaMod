using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems
{
	public class SupernovaKeybinds : ModSystem
	{
		public static ModKeybind RingAbilityButton { get; private set; }

		public override void Load()
		{
			RingAbilityButton = KeybindLoader.RegisterKeybind(Mod, "Ring Ability", "Q");
		}

		public override void Unload()
		{
			RingAbilityButton = null;
		}
	}
}
