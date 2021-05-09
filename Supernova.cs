using Microsoft.Xna.Framework.Graphics;
using Supernova.UI;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Supernova
{
	public class Supernova : Mod
	{
		/* Ring Code */
		public static ModHotKey ringAbilityButton;
		public override void Load()
		{
			// Registers a new hotkey
			ringAbilityButton = RegisterHotKey("Ring Ability", "Q");
		}

		/// <summary>
		/// For mod compatibility
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public override object Call(params object[] args)
		{
			try
			{
				string keyword = args[0] as string;
				if (string.IsNullOrEmpty(keyword))
					return null;

				switch (keyword)
				{
				}
			}
			catch
			{
				return null;
			}
			return null;
		}

		/* Add Boss Checklist */
		public override void PostSetupContent()
		{
			/// [Terraria bosses]
			// SlimeKing = 1f;
			// EyeOfCthulhu = 2f;
			// EaterOfWorlds = 3f;
			// QueenBee = 4f;
			// Skeletron = 5f;
			// WallOfFlesh = 6f;
			// TheTwins = 7f;
			// TheDestroyer = 8f;
			// SkeletronPrime = 9f;
			// Plantera = 10f;
			// Golem = 11f;
			// DukeFishron = 12f;
			// LunaticCultist = 13f;
			// Moonlord = 14f;

			Mod bossChecklist = ModLoader.GetMod("BossChecklist");
			if (bossChecklist != null)
			{
				//bossChecklist.Call("AddBossWithInfo", "Harbinger of Annihilation", 1.8f, (Func<bool>)(() => SupernovaWorld.downedHarbingerOfAnnihilation), "Kill a cosmic anomaly (spawns in the sky)");
				bossChecklist.Call("AddBoss", 1.8f,
					new List<int> { ModContent.NPCType<Npcs.Bosses.HarbingerOfAnnihilation.HarbingerOfAnnihilation>() },
					this,
					"Harbinger of Annihilation",
					(Func<bool>)(() => SupernovaWorld.downedHarbingerOfAnnihilation),
					0, //ModContent.NPCType<Npcs.PreHardmode.CosmicAnomaly>(),
					new List<int> {
						ModContent.ItemType<Npcs.Bosses.HarbingerOfAnnihilation.HarbingersCrest>(),
						ModContent.ItemType<Npcs.Bosses.HarbingerOfAnnihilation.HarbingersKnell>(),
						ModContent.ItemType<Npcs.Bosses.HarbingerOfAnnihilation.HarbingersPick>(),
						ModContent.ItemType<Npcs.Bosses.HarbingerOfAnnihilation.HarbingersSlicer>(),
					},
					new List<int> {
						ModContent.ItemType<Npcs.Bosses.HarbingerOfAnnihilation.HarbingersCrest>(),
						ModContent.ItemType<Npcs.Bosses.HarbingerOfAnnihilation.HarbingersKnell>(),
						ModContent.ItemType<Npcs.Bosses.HarbingerOfAnnihilation.HarbingersPick>(),
						ModContent.ItemType<Npcs.Bosses.HarbingerOfAnnihilation.HarbingersSlicer>(),
					},
					"Kill a Cosmic Anomaly in Space"
				);
				//bossChecklist.Call("AddBossWithInfo", "Flying Terror", 3.7f, (Func<bool>)(() => SupernovaWorld.downedFlyingTerror), "Use a [i:" + ItemType("HorridChunk") + "] at night");
				bossChecklist.Call("AddBoss", 3.7f,
					new List<int> { ModContent.NPCType<Npcs.Bosses.FlyingTerror.FlyingTerror>() },
					this,
					"Flying Terror",
					(Func<bool>)(() => SupernovaWorld.downedFlyingTerror),
					ItemType("HorridChunk"),
					new List<int> {
						ModContent.ItemType<Npcs.Bosses.FlyingTerror.FlyingTerrorBag>(),
						ModContent.ItemType<Npcs.Bosses.FlyingTerror.TerrorWing>()
					},
					new List<int> { 
						ModContent.ItemType<Npcs.Bosses.FlyingTerror.FlyingTerrorBag>(),
						ModContent.ItemType<Npcs.Bosses.FlyingTerror.TerrorInABottle>(),
						ModContent.ItemType<Npcs.Bosses.FlyingTerror.BlunderBuss>(),
						ModContent.ItemType<Npcs.Bosses.FlyingTerror.TerrorCleaver>(),
						ModContent.ItemType<Npcs.Bosses.FlyingTerror.TerrorKnife>(),
						ModContent.ItemType<Npcs.Bosses.FlyingTerror.TerrorRecurve>(),
						ModContent.ItemType<Npcs.Bosses.FlyingTerror.TerrorTome>(),
					},
					"Use a [i:" + ItemType("HorridChunk") + "] at night"
				);

				//bossChecklist.Call("AddBossWithInfo", "Stone MantaRay", 5.4f, (Func<bool>)(() => SupernovaWorld.downedStoneManta), "Use a [i:" + ItemType("MantaFood") + "] in the Underground stone layer");
				bossChecklist.Call("AddBoss", 5.4f,
					new List<int> { ModContent.NPCType<Npcs.Bosses.StoneMantaRay.StoneMantaRay>() },
					this,
					"Stone Manta Ray",
					(Func<bool>)(() => SupernovaWorld.downedStoneManta),
					ItemType("MantaFood"),
					new List<int> {
						ModContent.ItemType<Npcs.Bosses.StoneMantaRay.StoneGlove>(),
						ModContent.ItemType<Npcs.Bosses.StoneMantaRay.StoneRepeater>(),
						ModContent.ItemType<Npcs.Bosses.StoneMantaRay.SurgestoneSword>(),
					},
					new List<int> {
						ModContent.ItemType<Npcs.Bosses.StoneMantaRay.StoneGlove>(),
						ModContent.ItemType<Npcs.Bosses.StoneMantaRay.StoneRepeater>(),
						ModContent.ItemType<Npcs.Bosses.StoneMantaRay.SurgestoneSword>(),
					},
					"Use a [i:" + ItemType("MantaFood") + "] Underground"
				);
				//bossChecklist.Call("AddToBossLoot", "ZephyrMod", "OculusOfCthulhu", new List<int> { ModContent.ItemType<Items.TreasureBags.OculusOfCthulhuTreasureBag>() });
				//bossChecklist.Call("AddToBossSpawnItems", "ZephyrMod", "OculusOfCthulhu", new List<int> { ModContent.ItemType<Items.SummoningItems.EvilishLookingEye>() });

				/*bossChecklist.Call("AddBossWithInfo", "Cosmic Collective", 6.9f, (Func<bool>)(() => SupernovaWorld.downedCosmicCollective), "Use a [i:" + ItemType("CosmicEgg") + "]");
				bossChecklist.Call("AddBossWithInfo", "Helios the Infernal Overlord", 8.4f, (Func<bool>)(() => SupernovaWorld.downedHelios), "Use a [i:" + ItemType("infernalRitualStone") + "] in hell");
				bossChecklist.Call("AddBossWithInfo", "Cocytus", 10.9f, (Func<bool>)(() => SupernovaWorld.downedCocytus), "Use a [i:" + ItemType("FrozenSkull") + "] at night in the snow biome");
				bossChecklist.Call("AddBossWithInfo", "Shimmering Light Metatron", 12.7f, (Func<bool>)(() => SupernovaWorld.downedShimmeringLightMetatron), "Use a [i:" + ItemType("UnholyPact") + "] at daytime in the hallowed biome");

				bossChecklist.Call("AddBossWithInfo", "Deathbringer", 15f, (Func<bool>)(() => SupernovaWorld.downedDeathbringer), "Use a [i:" + ItemType("HiTechBeacon") + "]");*/
			}
		}

		#region [Ring Slot]
		public const string RING_SLOT_BACKGROUND_TEXTURE = "RingSlotBackground";

		public static bool AllowAccessorySlots = false;
		public static bool SlotsNextToAccessories = true;

		private static List<Func<bool>> RightClickOverrides;

		public override void Unload()
		{
			if (RightClickOverrides != null)
			{
				RightClickOverrides.Clear();
				RightClickOverrides = null;
			}
		}

		public override void PostDrawInterface(SpriteBatch spriteBatch)
		{
			RingSlotPlayer player = Main.LocalPlayer.GetModPlayer<RingSlotPlayer>();
			player.Draw(spriteBatch);
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			PacketMessageType message = (PacketMessageType)reader.ReadByte();
			byte player = reader.ReadByte();
			RingSlotPlayer modPlayer = Main.player[player].GetModPlayer<RingSlotPlayer>();

			switch (message)
			{
				case PacketMessageType.All:
					modPlayer.EquipSlot.Item = ItemIO.Receive(reader);
					modPlayer.VanitySlot.Item = ItemIO.Receive(reader);
					modPlayer.DyeSlot.Item = ItemIO.Receive(reader);
					if (Main.netMode == 2)
					{
						ModPacket packet = GetPacket();
						packet.Write((byte)PacketMessageType.All);
						packet.Write(player);
						ItemIO.Send(modPlayer.EquipSlot.Item, packet);
						ItemIO.Send(modPlayer.VanitySlot.Item, packet);
						ItemIO.Send(modPlayer.DyeSlot.Item, packet);
						packet.Send(-1, whoAmI);
					}
					break;
				case PacketMessageType.EquipSlot:
					modPlayer.EquipSlot.Item = ItemIO.Receive(reader);
					if (Main.netMode == 2)
					{
						modPlayer.SendSingleItemPacket(PacketMessageType.EquipSlot, modPlayer.EquipSlot.Item, -1, whoAmI);
					}
					break;
				case PacketMessageType.VanitySlot:
					modPlayer.VanitySlot.Item = ItemIO.Receive(reader);
					if (Main.netMode == 2)
					{
						modPlayer.SendSingleItemPacket(PacketMessageType.VanitySlot, modPlayer.VanitySlot.Item, -1, whoAmI);
					}
					break;
				case PacketMessageType.DyeSlot:
					modPlayer.DyeSlot.Item = ItemIO.Receive(reader);
					if (Main.netMode == 2)
					{
						modPlayer.SendSingleItemPacket(PacketMessageType.DyeSlot, modPlayer.DyeSlot.Item, -1, whoAmI);
					}
					break;
				default:
					Logger.InfoFormat("[Ring Slot] Unknown message type: {0}", message);
					break;
			}
		}

		public static bool OverrideRightClick()
		{
			foreach (var func in RightClickOverrides)
				if (func())
					return true;
			return false;
		}
		#endregion
	}
}