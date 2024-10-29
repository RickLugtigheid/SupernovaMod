using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace SupernovaMod
{
	public static class SupernovaModShaders
	{
        // public static Filter ExampleShader => Filters.Scene["SupernovaMod:ExampleShader"];
        public static Filter ShockwaveShader => Filters.Scene["SupernovaMod:shockwave"];

        public static void LoadEffects()
		{
			AssetRepository assets = Supernova.Instance.Assets;

            // TODO: Add error handling so when one shader does not
            // load, others are able to load.
			LoadScreenShaders(assets);
            LoadArmorShaders(assets);
        }

		public static void LoadScreenShaders(AssetRepository assets)
		{
            // Shockwave shader
            //
            Asset<Effect> shockwaveShader = assets.Request<Effect>("Assets/Effects/ScreenFilters/Shockwave", AssetRequestMode.ImmediateLoad);
            Filters.Scene["SupernovaMod:shockwave"] = new Filter(new(shockwaveShader, "Shockwave"), EffectPriority.VeryHigh);
            Filters.Scene["SupernovaMod:shockwave"].Load();
        }

		public static void LoadArmorShaders(AssetRepository assets)
		{
            // See: https://github.com/tModLoader/tModLoader/wiki/Expert-Shader-Guide#using-your-shader

            // Crystaline Dye shader
            //
            Asset<Effect> dyeShader = assets.Request<Effect>("Assets/Effects/Armor/CrystalineDye");
            GameShaders.Armor.BindShader(ModContent.ItemType<Content.Items.Dye.CrystalineDye>(), new ArmorShaderData(dyeShader, "ArmorCrystalEffect"))
                .UseColor(78, 4, 57)
                .UseSecondaryColor(139, 18, 97);
        }
    }
}
