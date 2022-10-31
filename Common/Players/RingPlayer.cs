using Supernova.Api;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Supernova.Common.Players
{
	public class RingPlayer : ModPlayer
	{
		internal static AccessorySlotLoader Loader => LoaderManager.Get<AccessorySlotLoader>();

		public static float ringCooldownMulti = 1;

		public override void PreUpdate()
		{
			// Reset the ring cooldown multi to 1
			// We do this so any accessories (or other) effects don't keep aplying
			//
			ringCooldownMulti = 1;
			base.PreUpdate();
		}

		public override void PostUpdateEquips()
		{
			// Check if a ring is equiped by the player
			//
			if (HasRing(out SupernovaRing equipedRing))
			{
				try
				{
					int ringCooldownBuffID = ModContent.BuffType<Content.Global.Buffs.RingCooldown>();

					// Check if the player doesn't have the ring cooldown debuff
					//
					if (!Player.HasBuff(ringCooldownBuffID))
					{
						if (equipedRing.CanRingActivate(Player) && Supernova.ringAbilityButton.JustPressed)
						{
							// Call the ring activate event
							//
							equipedRing?.OnRingActivate(Player);

							// After the ring is activated give the player a cooldown
							//
							Player.AddBuff(ringCooldownBuffID, (int)Math.Ceiling(equipedRing.Cooldown * ringCooldownMulti));
						}
					}
					else
					{
						// Call the ring cooldown effect for if the ring should give an effect when cooling down
						//
						equipedRing.OnRingCooldown(Player.buffTime[Player.FindBuffIndex(ringCooldownBuffID)], Player);
					}
				}
				catch (Exception ex)
				{
					Main.NewText("Supernova: Error '" + ex.Message+ "' when using the '" + equipedRing.Name + "' ring", Main.errorColor);
					Logging.PublicLogger.Error("Supernova: " + ex);
				}
			}

			base.PostUpdateEquips();
		}

		/// <summary>
		/// Checks if the player has equiped a ring
		/// </summary>
		/// <returns>If the player has a ring</returns>
		public bool HasRing(out SupernovaRing equipedRing)
		{
			// Get the item currently in our ring slot
			if (TryGetRingSlot(out ModAccessorySlot ringSlot))
			{
				Item ringSlotItem = ringSlot.FunctionalItem;

				// Check if a ring is equiped
				//
				if (ItemIsRing(ringSlotItem))
				{
					equipedRing = ringSlotItem.ModItem as SupernovaRing;
					return true;
				}
			}

			// No ring is equiped
			//
			equipedRing = null;
			return false;
		}

		public bool TryGetRingSlot(out ModAccessorySlot ringSlot)
		{
			try
			{
				ringSlot = Loader.Get(Loader.VanillaCount, Player);
				return true;
			}
			catch
			{
				ringSlot = null;
				return false;
			}
		}

		public static bool ItemIsRing(Item item) => item != null && item.ModItem != null && item.ModItem.GetType().IsSubclassOf(typeof(SupernovaRing));
	}

	public class SupernovaRingSlot : ModAccessorySlot
	{
		public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
		{
			return RingPlayer.ItemIsRing(checkItem);
		}

		// Designates our slot to be a priority for putting wings in to. NOTE: use ItemLoader.CanEquipAccessory if aiming for restricting other slots from having wings!
		public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo)
		{
			return RingPlayer.ItemIsRing(item);
		}

		public override bool IsEnabled()
		{
			//return base.IsEnabled();
			return true;
		}

		// Overrides the default behaviour where a disabled accessory slot will allow retrieve items if it contains items
		public override bool IsVisibleWhenNotEnabled()
		{
			return false; // We set to false to just not display if not Enabled. NOTE: this does not affect behavour when mod is unloaded!
		}

		// Icon textures. Nominal image size is 32x32. Will be centered on the slot.
		public override string FunctionalTexture => "Supernova/Content/UI/RingSlotBackground";

		// Can be used to modify stuff while the Mouse is hovering over the slot.
		public override void OnMouseHover(AccessorySlotType context)
		{
			// We will modify the hover text while an item is not in the slot, so that it says "Rings".
			switch (context)
			{
				case AccessorySlotType.FunctionalSlot:
					Main.hoverItemName = "Rings";
					break;
				case AccessorySlotType.VanitySlot:
					Main.hoverItemName = "Vanity Rings";
					break;
				case AccessorySlotType.DyeSlot:
					Main.hoverItemName = "Rings Dye";
					break;
			}
		}
	}
}