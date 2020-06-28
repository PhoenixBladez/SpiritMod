using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Backgrounds
{
	public class ReachSurfaceBgStyle : ModSurfaceBgStyle
	{


		public override int ChooseMiddleTexture() => mod.GetBackgroundSlot("Backgrounds/ReachBiomeSurfaceMid");

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
		{
			scale *= .86f;
			b -= 240;
			return mod.GetBackgroundSlot("Backgrounds/ReachBiomeSurfaceClose");
		}

		public override bool ChooseBgStyle() => !Main.gameMenu && Main.LocalPlayer.GetSpiritPlayer().ZoneReach;

		// Use this to keep far Backgrounds like the mountains.
		public override void ModifyFarFades(float[] fades, float transitionSpeed)
		{
			for(int i = 0; i < fades.Length; i++) {
				if(i == Slot) {
					fades[i] += transitionSpeed;
					if(fades[i] > 1f) {
						fades[i] = 1f;
					}
				} else {
					fades[i] -= transitionSpeed;
					if(fades[i] < 0f) {
						fades[i] = 0f;
					}
				}
			}
		}
	}
}