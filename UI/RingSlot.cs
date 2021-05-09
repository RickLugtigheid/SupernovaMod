using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;
using Terraria.UI;
using TerraUI.Objects;


/// <summary>
/// This ring slot was made with the help of the wingSlot mod:
/// https://github.com/abluescarab/tModLoader-WingSlot
/// </summary>

namespace Supernova.UI
{

	enum PacketMessageType : byte
	{
		EquipSlot,
		VanitySlot,
		DyeSlot,
		All
	}
	internal class RingSlotPlayer : ModPlayer
	{
		private const string TAG_HIDDEN			= "hidden";
		private const string TAG_RING			= "ring";
		private const string TAG_RING_VANITY	= "vanityring";
		private const string TAG_RING_DYE		= "ringdye";

		public UIItemSlot EquipSlot { get; private set; }
		public UIItemSlot VanitySlot { get; private set; }
		public UIItemSlot DyeSlot { get; private set; }

		public bool WingSlotModActive { get; private set; }

		/// <summary>
		/// If the client is cloned we make sure the clone has the same rings in its slots
		/// </summary>
		/// <param name="clientClone"></param>
		public override void clientClone(ModPlayer clientClone)
		{
			RingSlotPlayer clone = clientClone as RingSlotPlayer;

			// Check if the clone is of type 'RingSlotPlayer'
			if (clone == null) return;

			// Set our slots
			clone.EquipSlot.Item	= EquipSlot.Item;
			clone.VanitySlot.Item	= VanitySlot.Item;
			clone.DyeSlot.Item		= DyeSlot.Item;
		}

		/// <summary>
		/// Handle the ring slot on client changes
		/// </summary>
		/// <param name="clientPlayer"></param>
		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			RingSlotPlayer oldPlayer = clientPlayer as RingSlotPlayer;

			// Check if the oldPlayer is of type 'RingSlotPlayer'
			if (oldPlayer == null) return;

			// Update the items in the slot if changed
			if (oldPlayer.EquipSlot.Item.IsNotTheSameAs(EquipSlot.Item))
				SendSingleItemPacket(PacketMessageType.EquipSlot, EquipSlot.Item, -1, player.whoAmI); // Send new item to the server
			if (oldPlayer.VanitySlot.Item.IsNotTheSameAs(VanitySlot.Item))
				SendSingleItemPacket(PacketMessageType.VanitySlot, VanitySlot.Item, -1, player.whoAmI); // Send new item to the server
			if (oldPlayer.EquipSlot.Item.IsNotTheSameAs(DyeSlot.Item))
				SendSingleItemPacket(PacketMessageType.DyeSlot, DyeSlot.Item, -1, player.whoAmI); // Send new item to the server
		}
		/// <summary>
		/// Send a single packet with an item to the server
		/// </summary>
		/// <param name="message"></param>
		/// <param name="item"></param>
		/// <param name="toWho"></param>
		/// <param name="fromWho"></param>
		internal void SendSingleItemPacket(PacketMessageType message, Item item, int toWho, int fromWho)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)message);
			packet.Write((byte)player.whoAmI);
			ItemIO.Send(item, packet);
			packet.Send(toWho, fromWho);
		}

		/// <summary>
		/// Syncs the player
		/// </summary>
		/// <param name="toWho"></param>
		/// <param name="fromWho"></param>
		/// <param name="newPlayer"></param>
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)PacketMessageType.All);
			packet.Write((byte)player.whoAmI);
			ItemIO.Send(EquipSlot.Item, packet);
			ItemIO.Send(VanitySlot.Item, packet);
			ItemIO.Send(DyeSlot.Item, packet);
			packet.Send(toWho, fromWho);
		}

		public override void Initialize()
		{
			WingSlotModActive = ModLoader.GetMod("WingSlot") != null;
			EquipSlot = new UIItemSlot(Vector2.Zero, context: ItemSlot.Context.EquipAccessory, hoverText: "Ring",
										conditions: Slot_Conditions, drawBackground: Slot_DrawBackground, scaleToInventory: true);
			VanitySlot = new UIItemSlot(Vector2.Zero, context: ItemSlot.Context.EquipAccessoryVanity, hoverText:
										Language.GetTextValue("LegacyInterface.11") + " Ring",
										conditions: Slot_Conditions, drawBackground: Slot_DrawBackground, scaleToInventory: true);
			DyeSlot = new UIDyeItemSlot(Vector2.Zero, context: ItemSlot.Context.EquipDye, conditions: RingDyeSlot_Conditions,
										drawBackground: RingDyeSlot_DrawBackground, scaleToInventory: true);
			
			VanitySlot.Partner = EquipSlot;
			EquipSlot.BackOpacity = VanitySlot.BackOpacity = DyeSlot.BackOpacity = .8f;

			InitializeRing();
		}


		public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
		{
			/// TODO: Use dye on wings
			/// Like here https://github.com/abluescarab/tModLoader-WingSlot/blob/master/WingSlotPlayer.cs#L91
			base.ModifyDrawInfo(ref drawInfo);
		}

		/// <summary>
		/// Update player with the equipped Ring.
		/// </summary>
		public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
		{
			/// TODO: Make sure we can use the ring like intended
			/// So check if we have a ring and than use the ring when 'Q' is pressed
			Item ring		= EquipSlot.Item;
			Item vanityRing = VanitySlot.Item;

			if(ring.stack > 0)
			{

			}
		}

		/// <summary>
		/// Save the equipted rings
		/// </summary>
		/// <returns></returns>
		public override TagCompound Save()
		{
			return new TagCompound {
				{ TAG_HIDDEN,		EquipSlot.ItemVisible },
				{ TAG_RING,			ItemIO.Save(EquipSlot.Item) },
				{ TAG_RING_VANITY,	ItemIO.Save(VanitySlot.Item) },
				{ TAG_RING_DYE,		ItemIO.Save(DyeSlot.Item) }
			};
		}
		/// <summary>
		/// Load our equipted rings
		/// </summary>
		/// <param name="tag"></param>
		public override void Load(TagCompound tag)
		{
			SetRing(false, ItemIO.Load(
				tag.GetCompound(TAG_RING)
			));
			SetRing(true, ItemIO.Load(
				tag.GetCompound(TAG_RING_VANITY)
			));
			SetDye(ItemIO.Load(
				tag.GetCompound(TAG_RING_DYE)	
			));
		}

		private void InitializeRing()
		{
			EquipSlot.Item	= new Item();
			VanitySlot.Item = new Item();
			DyeSlot.Item	= new Item();

			EquipSlot.Item.SetDefaults(0, true);
			VanitySlot.Item.SetDefaults(0, true);
			DyeSlot.Item.SetDefaults(0, true);
		}
		private void SetRing(bool isVanity, Item ring)
		{
			if (isVanity)
			{
				VanitySlot.Item = ring.Clone();
			}
			else
			{
				EquipSlot.Item = ring.Clone();
			}
		}
		private void ClearRing(bool isVanity)
		{
			if(isVanity)
			{
				EquipSlot.Item = new Item();
				EquipSlot.Item.SetDefaults();
			}
			else
			{
				VanitySlot.Item = new Item();
				VanitySlot.Item.SetDefaults();
			}
		}

		private void SetDye(Item item) => DyeSlot.Item = item.Clone();
		private void ClearDye() => DyeSlot.Item = new Item();

		/// <summary>
		/// Equip a ring.
		/// </summary>
		/// <param name="isVanity">whether the ring should go in the vanity slot</param>
		/// <param name="item">ring</param>
		public void EquipRing(bool isVanity, Item item)
		{
			UIItemSlot slot = (isVanity ? VanitySlot : EquipSlot);
			int fromSlot = Array.FindIndex(player.inventory, i => i == item);

			// from inv to slot
			if (fromSlot < 0)
			{
				return;
			}

			item.favorited = false;
			player.inventory[fromSlot] = slot.Item.Clone();
			Main.PlaySound(SoundID.Grab);
			Recipe.FindRecipes();
			SetRing(isVanity, item);
		}

		/// <summary>
		/// Equip a dye.
		/// </summary>
		/// <param name="item">dye to equip</param>
		public void EquipDye(Item item)
		{
			int fromSlot = Array.FindIndex(player.inventory, i => i == item);

			// from inv to slot
			if (fromSlot < 0)
				return;

			item.favorited = false;
			player.inventory[fromSlot] = DyeSlot.Item.Clone();
			Main.PlaySound(SoundID.Grab);
			Recipe.FindRecipes();
			SetDye(item);
		}

		/// <summary>
		/// Get the ring that a dye should be applied to.
		/// </summary>
		/// <returns>dyed wings</returns>
		public Item GetDyedRing()
		{
			if (VanitySlot.Item.stack > 0)
				return VanitySlot.Item;

			if (EquipSlot.Item.stack > 0)
				return EquipSlot.Item;

			return new Item();
		}

		/// <summary>
		/// Draw the wing slot backgrounds.
		/// </summary>
		private void Slot_DrawBackground(UIObject sender, SpriteBatch spriteBatch)
		{
			UIItemSlot slot = (UIItemSlot)sender;

			if (ShouldDrawSlots())
			{
				slot.OnDrawBackground(spriteBatch);

				if (slot.Item.stack == 0)
				{
					Texture2D tex = mod.GetTexture(Supernova.RING_SLOT_BACKGROUND_TEXTURE);
					Vector2 origin = tex.Size() / 2f * Main.inventoryScale;
					Vector2 position = slot.Rectangle.TopLeft();

					spriteBatch.Draw(
						tex,
						position + (slot.Rectangle.Size() / 2f) - (origin / 2f),
						null,
						Color.White * 0.35f,
						0f,
						origin,
						Main.inventoryScale,
						SpriteEffects.None,
						0f); // layer depth 0 = front
				}
			}
		}

		/// <summary>
		/// Control what can be placed in the ring slots.
		/// </summary>
		private static bool Slot_Conditions(Item item)
		{
			if (item.modItem as RingBase == null)
				return false;
			return true;
		}
		/// <summary>
		/// Draw the ring dye slot background.
		/// </summary>
		private static void RingDyeSlot_DrawBackground(UIObject sender, SpriteBatch spriteBatch)
		{
			UIItemSlot slot = (UIItemSlot)sender;

			if (!ShouldDrawSlots())
				return;

			slot.OnDrawBackground(spriteBatch);

			if (slot.Item.stack != 0)
				return;

			Texture2D tex = Main.extraTexture[54];
			Rectangle rectangle = tex.Frame(3, 6, 1 % 3);
			rectangle.Width -= 2;
			rectangle.Height -= 2;
			Vector2 origin = rectangle.Size() / 2f * Main.inventoryScale;
			Vector2 position = slot.Rectangle.TopLeft();

			spriteBatch.Draw(
				tex,
				position + (slot.Rectangle.Size() / 2f) - (origin / 2f),
				rectangle,
				Color.White * 0.35f,
				0f,
				origin,
				Main.inventoryScale,
				SpriteEffects.None,
				0f); // layer depth 0 = front
		}
		/// <summary>
		/// Control what can be placed in the wing dye slot.
		/// </summary>
		private static bool RingDyeSlot_Conditions(Item item) =>  item.dye > 0 && item.hairDye < 0;

		/// <summary>
		/// Draw the ring slots.
		/// </summary>
		/// <param name="spriteBatch">drawing SpriteBatch</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			if (!ShouldDrawSlots())
				return;

			int mapH = 0;
			int rX;
			int rY;
			float origScale = Main.inventoryScale;

			Main.inventoryScale = 0.85f;

			if (Main.mapEnabled)
			{
				if (!Main.mapFullscreen && Main.mapStyle == 1)
					mapH = 256;
			}

			if (!Supernova.SlotsNextToAccessories)
			{
				if (Main.mapEnabled)
				{
					if ((mapH + 600) > Main.screenHeight)
						mapH = Main.screenHeight - 600;
				}

				rX = Main.screenWidth - 92 - (47 * 2);
				rY = mapH + 174;

				if (Main.netMode == NetmodeID.MultiplayerClient)
					rX -= 47;
			}
			else
			{
				if (Main.mapEnabled)
				{
					int adjustY = 600;

					if (Main.player[Main.myPlayer].ExtraAccessorySlotsShouldShow)
						adjustY = 610 + PlayerInput.UsingGamepad.ToInt() * 30;

					if ((mapH + adjustY) > Main.screenHeight)
						mapH = Main.screenHeight - adjustY;
				}

				int slotCount = 7 + Main.player[Main.myPlayer].extraAccessorySlots;

				if ((Main.screenHeight < 900) && (slotCount >= 8))
				{
					slotCount = 7;
				}

				rX = Main.screenWidth - 92 - 14 - (47 * 3) - (int)(Main.extraTexture[58].Width * Main.inventoryScale);
				rY = (int)(mapH + 174 + 4 + slotCount * 56 * Main.inventoryScale);

				// If we also have the wing slot mod we up the y pos so we are above the wing slots
				if (WingSlotModActive)
					rY -= (int)EquipSlot.Size.Y;
			}

			EquipSlot.Position = new Vector2(rX, rY);
			VanitySlot.Position = new Vector2(rX -= 47, rY);
			DyeSlot.Position = new Vector2(rX - 47, rY);

			VanitySlot.Draw(spriteBatch);
			EquipSlot.Draw(spriteBatch);
			DyeSlot.Draw(spriteBatch);

			Main.inventoryScale = origScale;

			EquipSlot.Update();
			VanitySlot.Update();
			DyeSlot.Update();
		}

		/// <summary>
		/// Whether to draw the UIItemSlots.
		/// </summary>
		/// <returns>whether to draw the slots</returns>
		// private static bool ShouldDrawSlots(out int slotLocation) {
		private static bool ShouldDrawSlots() =>  Main.playerInventory && ((Supernova.SlotsNextToAccessories && Main.EquipPage == 0) || (!Supernova.SlotsNextToAccessories && Main.EquipPage == 2));
	}
	public class RingConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[DefaultValue(true)]
		[Tooltip("Place the new slots next to accessories or next to\nunique equipment (pets, minecart, etc.)")]
		[Label("Slots next to accessories")]
		public bool SlotsNextToAccessories;

		[DefaultValue(false)]
		[Label("Allow equipping in accessory slots")]
		public bool AllowAccessorySlots;

		public override void OnChanged()
		{
			Supernova.SlotsNextToAccessories = SlotsNextToAccessories;
			Supernova.AllowAccessorySlots = AllowAccessorySlots;
		}
	}
}
