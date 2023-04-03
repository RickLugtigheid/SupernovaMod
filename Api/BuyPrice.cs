using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SupernovaMod.Api
{
	public static class BuyPrice
	{
		public static readonly int SoldRarityGreen = Item.buyPrice(0, 18, 0, 0);

		public static readonly int RarityWhite = Item.buyPrice(0, 0, 50, 0);
		public static readonly int RarityBlue = Item.buyPrice(0, 1, 0, 0);
		public static readonly int RarityGreen = Item.buyPrice(0, 2, 0, 0);
		public static readonly int RarityOrange = Item.buyPrice(0, 4, 0, 0);
		public static readonly int RarityLightRed = Item.buyPrice(0, 12, 0, 0);
		public static readonly int RarityPink = Item.buyPrice(0, 24, 0, 0);
		public static readonly int RarityLightPurple= Item.buyPrice(0, 36, 0, 0);
		public static readonly int RarityLime = Item.buyPrice(0, 48, 0, 0);
		public static readonly int RarityYellow = Item.buyPrice(0, 60, 0, 0);
		public static readonly int RarityCyan = Item.buyPrice(0, 80, 0, 0);
		public static readonly int RarityRed = Item.buyPrice(1, 0, 0, 0);
		public static readonly int RarityPurple = Item.buyPrice(1, 10, 0, 0);
	}
}
