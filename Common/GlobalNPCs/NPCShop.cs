using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Common.GlobalNPCs
{
    public class NPCShop : GlobalNPC
	{
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			switch (type)
			{
				case NPCID.ArmsDealer:
					SetupShopArmsDealer(shop, ref nextSlot);
					break;
			}

			base.SetupShop(type, shop, ref nextSlot);
		}

		/// <summary>
		/// Add custom items to the arms dealer shop
		/// </summary>
		/// <param name="type"></param>
		/// <param name="shop"></param>
		/// <param name="nextSlot"></param>
		private void SetupShopArmsDealer(Chest shop, ref int nextSlot)
		{
			// Add the FirearmManual to the shop
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Materials.FirearmManual>());
			nextSlot++;
		}
	}
}
