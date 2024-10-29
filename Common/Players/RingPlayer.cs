using SupernovaMod.Common.Systems;
using SupernovaMod.Content.Buffs.Cooldowns;
using SupernovaMod.Content.Items.Rings.BaseRings;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace SupernovaMod.Common.Players
{
    public class RingPlayer : ModPlayer
	{
		private readonly static int _ringCooldownBuffType = ModContent.BuffType<RingCooldown>();
		
		/// <summary>
		/// If our ring is on a cooldown
		/// </summary>
		public bool RingOnCooldown => Player.HasBuff(_ringCooldownBuffType);

		#region Ring Animation Properties
		private int _ringAnimationFrame = 0;
		/// <summary>
		/// If our ring is animating
		/// </summary>
		public bool RingAnimationActive => _ringAnimationFrame > 0;
		#endregion

		public override void PostUpdateEquips()
		{
			ResourcePlayer resourcePlayer = Player.GetModPlayer<ResourcePlayer>();
			// Check if a ring is equiped by the player
			//
			if (HasRing(out SupernovaRingItem equipedRing))
			{
				try
				{
					// Check if the player doesn't have the ring cooldown debuff
					//
					if (!RingOnCooldown && equipedRing.CanRingActivate(this))
					{
						// Check if our 'ringAbility' button was pressed and we are not animating
						//
						if (SupernovaKeybinds.RingAbilityButton.JustPressed && !RingAnimationActive)
						{
							SoundEngine.PlaySound(SoundID.DD2_PhantomPhoenixShot, Player.Center);
							// When the ringAbilityButton is pressed we start our ring animation
							equipedRing.UseAnimation(Player);
							_ringAnimationFrame = equipedRing.MaxAnimationFrames; // Start our animation
						}
						// Else when animating
						//
						else if (RingAnimationActive)
						{
							// Update our ring animation until done
							equipedRing.RingUseAnimation(Player, _ringAnimationFrame);
							_ringAnimationFrame--;

							// On our last frame activate the ring
							//
							if (!RingAnimationActive)
							{
								// Activate our ring when our animation is done
								equipedRing.RingActivate(Player, resourcePlayer.ringPower);

								// After the ring is activated give the player a cooldown
								int cooldown = equipedRing.Cooldown;
								cooldown = (int)(cooldown * equipedRing.coolRegen);
								Player.AddBuff(_ringCooldownBuffType, cooldown);
							}
						}
					}
					else if (RingOnCooldown)
					{
						// Call the ring cooldown effect for if the ring should give an effect when cooling down
						//
						equipedRing?.OnRingCooldown(Player.buffTime[Player.FindBuffIndex(_ringCooldownBuffType)], Player);
					}
				}
				catch (Exception ex)
				{
					Main.NewText("SupernovaMod: Error '" + ex.Message+ "' when using the '" + equipedRing.Name + "' ring", Main.errorColor);
					Mod.Logger.Error("Error '" + ex.Message+ "' when using the '" + equipedRing.Name + "' ring");
					Mod.Logger.Error(ex);
				}
			}

			base.PostUpdateEquips();
		}

		/// <summary>
		/// Checks if the player has equiped a ring
		/// </summary>
		/// <returns>If the player has a ring</returns>
		public bool HasRing(out SupernovaRingItem equipedRing)
		{
			// Get the item currently in our ring slot
			//
			if (TryGetRingSlot(out ModAccessorySlot ringSlot) && ringSlot.IsEnabled())
			{
				Item ringSlotItem = ringSlot.FunctionalItem;

				// Check if a ring is equiped
				//
				if (ItemIsRing(ringSlotItem))
				{
					equipedRing = ringSlotItem.ModItem as SupernovaRingItem;
					return true;
				}
			}

			// No ring is equiped
			//
			equipedRing = null;
			return false;
		}

		private AccessorySlotLoader _slotLoader = null;
		/// <summary>
		/// Tries to get the ring Accessory slot.
		/// </summary>
		/// <param name="ringSlot"></param>
		/// <returns>If the ring Accessory slot was found</returns>
		public bool TryGetRingSlot(out ModAccessorySlot ringSlot)
		{
			try
			{
				ModAccessorySlotPlayer accPlayer = Player.GetModPlayer<ModAccessorySlotPlayer>();
				if (_slotLoader == null)
				{
					_slotLoader = LoaderManager.Get<AccessorySlotLoader>();
				}
				ringSlot = _slotLoader.Get(ModContent.GetInstance<SupernovaRingSlot>().Type, Player);
				return true;
			}
			catch
			{
				ringSlot = null;
				return false;
			}
		}

		/// <summary>
		/// Checks if an item is a ring item.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool ItemIsRing(Item item) => item != null && item.ModItem != null && item.ModItem.GetType().IsSubclassOf(typeof(SupernovaRingItem));
	}

	public class SupernovaRingSlot : ModAccessorySlot
	{
		// Icon textures. Nominal image size is 32x32. Will be centered on the slot.
		public override string FunctionalTexture => "SupernovaMod/Assets/Textures/RingSlotBackground";
		public override string Name => "SupernovaMod/SupernovaRingSlot";

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
			return ModContent.GetInstance<Configs.CommonConfig>().enableRingSlot;
		}

		// Overrides the default behaviour where a disabled accessory slot will allow retrieve items if it contains items
		public override bool IsVisibleWhenNotEnabled()
		{
			return false; // We set to false to just not display if not Enabled. NOTE: this does not affect behavour when mod is unloaded!
		}

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