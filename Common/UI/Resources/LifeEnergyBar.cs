using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using System.Collections.Generic;
using SupernovaMod.Common.Players;

namespace SupernovaMod.Common.UI.LifeEnergy
{
	internal class LifeEnergyBar : UIState
	{
		// For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
		// Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
		private UIText text;
		private UIElement area;
		private UIImage barFrame;
		private Color gradientA;
		private Color gradientB;

		public override void OnInitialize()
		{
			// Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
			// UIElement is invisible and has no padding.
			area = new UIElement();
			area.Left.Set(-area.Width.Pixels - 600, 1f); // Place the resource bar to the left of the hearts.
			area.Top.Set(30, 0f); // Placing it just a bit below the top of the screen.
			area.Width.Set(182, 0f); // We will be placing the following 2 UIElements within this 182x60 area.
			area.Height.Set(60, 0f);

			barFrame = new UIImage(ModContent.Request<Texture2D>("SupernovaMod/Common/UI/Resources/LifeEnergyFrame")); // Frame of our resource bar
			barFrame.Left.Set(22, 0f);
			barFrame.Top.Set(0, 0f);
			barFrame.Width.Set(138, 0f);
			barFrame.Height.Set(34, 0f);

			text = new UIText("0/0", 0.8f); // text to show stat
			text.Width.Set(138, 0f);
			text.Height.Set(34, 0f);
			text.Top.Set(40, 0f);
			text.Left.Set(0, 0f);

			gradientA = new Color(40, 145, 10); // A dark green
			gradientB = new Color(25, 255, 10); // A light green

			area.Append(text);
			area.Append(barFrame);
			Append(area);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			ResourcePlayer modPlayer = Main.LocalPlayer.GetModPlayer<ResourcePlayer>();

			// This prevents drawing unless we have a max life energy
			if (modPlayer.lifeEnergyMax2 == 0)
				return;

			base.Draw(spriteBatch);
		}

		// Here we draw our UI
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);

			ResourcePlayer modPlayer = Main.LocalPlayer.GetModPlayer<ResourcePlayer>();
			// Calculate quotient
			float quotient = (float)modPlayer.lifeEnergy / modPlayer.lifeEnergyMax2; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
			quotient = Utils.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

			// Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
			Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
			hitbox.X += 12;
			hitbox.Width -= 24;
			hitbox.Y += 8;
			hitbox.Height -= 16;

			// Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
			int left = hitbox.Left;
			int right = hitbox.Right;
			int steps = (int)((right - left) * quotient);
			for (int i = 0; i < steps; i += 1)
			{
				// float percent = (float)i / steps; // Alternate Gradient Approach
				float percent = (float)i / (right - left);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
			}
		}

		public override void Update(GameTime gameTime)
		{
			ResourcePlayer modPlayer = Main.LocalPlayer.GetModPlayer<ResourcePlayer>();

			// This prevents drawing unless we have a max life energy
			if (modPlayer.lifeEnergyMax2 == 0)
				return;

			text.SetText($"Life Energy: {(int)modPlayer.lifeEnergy} / {modPlayer.lifeEnergyMax2}");
		}
	}
	class LifeEnergyUISystem : ModSystem
	{
		private UserInterface LifeEnergyBarUserInterface;

		internal LifeEnergyBar LifeEnergyBar;

		public override void Load()
		{
			// All code below runs only if we're not loading on a server
			if (!Main.dedServ)
			{
				LifeEnergyBar = new();
				LifeEnergyBarUserInterface = new();
				LifeEnergyBarUserInterface.SetState(LifeEnergyBar);
			}
		}

		public override void UpdateUI(GameTime gameTime)
		{
			LifeEnergyBarUserInterface?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1)
			{
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
					"SupernovaMod: Life Energy Bar",
					delegate {
						LifeEnergyBarUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}
