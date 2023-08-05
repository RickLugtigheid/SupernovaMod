using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Common.GlobalNPCs
{
    public class ShopNPC : GlobalNPC
	{
		public override void ModifyShop(NPCShop shop)
		{
			// Call the ModifyShop method for the given NPC
			//
			switch (shop.NpcType)
			{
				case NPCID.ArmsDealer:
					ModifyShopArmsDealer(shop);
					break;
			}

			base.ModifyShop(shop);
		}

		/// <summary>
		/// Add custom items to the arms dealer shop
		/// </summary>
		/// <param name="type"></param>
		/// <param name="shop"></param>
		/// <param name="nextSlot"></param>
		private void ModifyShopArmsDealer(NPCShop shop)
		{
			// Add the FirearmManual to the shop
			//
			shop.Add(new Item(
				ModContent.ItemType<Content.Items.Materials.FirearmManual>()
			));
		}
	}
}
