using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupernovaMod.Api.ChestLoot
{
	public enum ChestFrameType
	{
		Chest,
		GoldChest,
		LockedGoldChest,
		ShadowChest,
		LockedShadowChest,
		Barrel,
		TrashCan,
		EbonwoodChest,
		RichMahoganyChest,  // Mushroom Chest?
		PearlwoodChest,
		IvyChest,           // Rich Mahogany Chest?
		IceChest,
		LivingWoodChest,
		SkywareChest,
		ShadewoodChest,
		WebCoveredChest,
		LihzahrdChest,
		WaterChest,
		JungleChest,
		CorruptionChest,
		CrimsonChest,
		HallowedChest,
		FrozenChest,
		LockedJungleChest,
		LockedCorruptionChest,
		LockedCrimsonChest,
		LockedHallowedChest,
		LockedFrozenChest,
		DynastyChest,
		HoneyChest,
		SteampunkChest,
		PalmWoodChest,
		MushroomChest,
		BorealWoodChest,
		SlimeChest,
		GreenDungeonChest,
		LockedGreenDungeonChest,
		PinkDungeonChest,
		LockedPinkDungeonChest,
		BlueDungeonChest,
		LockedBlueDungeonChest,
		BoneChest,
		CactusChest,
		FleshChest,
		ObsidianChest,
		PumpkinChest,
		SpookyChest,
		GlassChest,
		MartianChest,
		MeteoriteChest,
		GraniteChest,
		MarbleChest,
		CrystalChest,
		GoldenChest
	}
	/// <summary>
	/// The way the item should be injected to the chest.
	/// </summary>
	public enum ChestLootInjectRule { AddItem, ReplaceFirstItem }
}
