using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace SupernovaMod
{
	public static class SupernovaModTextures
	{
		public static Asset<Texture2D> Invisible { get; private set; }

		public static void LoadTextures()
		{
			AssetRepository assets = Supernova.Instance.Assets;

			Invisible = assets.Request<Texture2D>("Assets/Textures/InvisibleProjectile");
		}
	}
}
