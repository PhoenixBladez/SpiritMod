using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SpiritMod.Items.Pins;
using System.Collections.Generic;
using Terraria.Map;
using Terraria.ModLoader;

namespace SpiritMod.Utilities.Map
{
	internal class PinMapLayer : ModMapLayer
	{
		public static Dictionary<string, Asset<Texture2D>> Textures = null;

		public override void Draw(ref MapOverlayDrawContext context, ref string text)
		{
			if (Textures is null)
			{
				Textures = new Dictionary<string, Asset<Texture2D>>();
				Textures.Add("Blue", ModContent.Request<Texture2D>($"SpiritMod/Items/Pins/Textures/PinBlueMap"));
				Textures.Add("Red", ModContent.Request<Texture2D>($"SpiritMod/Items/Pins/Textures/PinRedMap"));
				Textures.Add("Green", ModContent.Request<Texture2D>($"SpiritMod/Items/Pins/Textures/PinGreenMap"));
				Textures.Add("Yellow", ModContent.Request<Texture2D>($"SpiritMod/Items/Pins/Textures/PinYellowMap"));
			}

			var pins = ModContent.GetInstance<PinWorld>().pins;
			foreach (var pair in pins)
			{
				var pos = pins.Get<Vector2>(pair.Key);
				context.Draw(Textures[pair.Key].Value, pos, new Terraria.UI.Alignment());
			}
		}
	}
}
