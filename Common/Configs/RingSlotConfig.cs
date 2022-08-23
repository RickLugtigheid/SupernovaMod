using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Supernova.Common.Configs
{
    public class RingSlotConfig : ModConfig
    {
        public enum Location
        {
            Accessories,
            Custom
        }

        private Location lastSlotLocation = Location.Accessories;

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static RingSlotConfig Instance;

        [Header("$Mods.RingSlot.SlotLocation_Header")]
        [DefaultValue(Location.Accessories)]
        [Label("$Mods.RingSlot.SlotLocation_Label")]
        [DrawTicks]
        public Location SlotLocation;

        [DefaultValue(false)]
        [Tooltip("$Mods.RingSlot.ShowCustomLocationPanel_Tooltip")]
        [Label("$Mods.RingSlot.ShowCustomLocationPanel_Label")]
        public bool ShowCustomLocationPanel;

        [DefaultValue(false)]
        [Tooltip("$Mods.RingSlot.ResetCustomSlotLocation_Tooltip")]
        [Label("$Mods.RingSlot.ResetCustomSlotLocation_Label")]
        public bool ResetCustomSlotLocation;

        public override void OnChanged()
        {
            /*if (Supernova.RingSlotUI == null)
                return;

            if (lastSlotLocation == Location.Custom && SlotLocation != Location.Custom)
                ShowCustomLocationPanel = false;

            if (ShowCustomLocationPanel)
                SlotLocation = Location.Custom;

            Supernova.RingSlotUI.Panel.Visible = ShowCustomLocationPanel;
            Supernova.RingSlotUI.Panel.CanDrag = ShowCustomLocationPanel;

            if (ResetCustomSlotLocation)
            {
                Supernova.RingSlotUI.ResetPosition();
                ResetCustomSlotLocation = false;
            }

            lastSlotLocation = SlotLocation;*/
        }
    }
}
