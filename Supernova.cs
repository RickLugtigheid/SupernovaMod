using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Supernova.Content.Global.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Supernova
{
	public class Supernova : Mod
	{
		public override void Load()
		{
			BindKeys();
		}

		/// <summary>
		/// Bind custom keys
		/// </summary>
		private void BindKeys()
		{
			KeybindLoader.RegisterKeybind(this, "Ring Ability", Microsoft.Xna.Framework.Input.Keys.Q);
		}
	}
}