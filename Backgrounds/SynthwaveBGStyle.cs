using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Backgrounds
{
	public class SynthwaveBGStyle : ModSurfaceBgStyle
	{
		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
		{
			scale *= .45f;
			b -= 120;
			return mod.GetBackgroundSlot("Backgrounds/SynthwaveBackground");
		}
		public override bool ChooseBgStyle() => !Main.gameMenu && Main.LocalPlayer.GetSpiritPlayer().ZoneSynthwave;
		// Use this to keep far Backgrounds like the mountains.
		public override void ModifyFarFades(float[] fades, float transitionSpeed)
		{
			for (int i = 0; i < fades.Length; i++) {
				if (i == Slot) {
					fades[i] += transitionSpeed;
					if (fades[i] > 1f) {
						fades[i] = 1f;
					}
				}
				else {
					fades[i] -= transitionSpeed;
					if (fades[i] < 0f) {
						fades[i] = 0f;
					}
				}
			}
		}
	}
}