using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Biomes
{
	public class DreamlandsWaterStyle : ModWaterStyle
	{
		private Asset<Texture2D> _rainTexture;
		public override void Load() {
			_rainTexture = Mod.Assets.Request<Texture2D>("Content/Biomes/DreamlandsRain");
		}

		public override int ChooseWaterfallStyle() {
			return ModContent.GetInstance<DreamlandsWaterfallStyle>().Slot;
		}

		public override int GetSplashDust() {
			//return ModContent.DustType<Water_Dreamlands>();
			return DustID.Water_Jungle;
		}

		public override int GetDropletGore() {
			//return ModContent.GoreType<DreamlandsDroplet>();
			return GoreID.WaterDripJungle;
		}

		public override void LightColorMultiplier(ref float r, ref float g, ref float b) {
			r = 1f;
			g = 1f;
			b = 1f;
		}

		public override Color BiomeHairColor() {
			return Color.Gray;
		}

		public override byte GetRainVariant() {
			return (byte)Main.rand.Next(3);
		}

		public override Asset<Texture2D> GetRainTexture() => _rainTexture;
	}
}