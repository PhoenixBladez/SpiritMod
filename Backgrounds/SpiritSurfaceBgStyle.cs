using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Backgrounds
{
	public class SpiritSurfaceBgStyle : ModSurfaceBgStyle
	{
		public override int ChooseFarTexture() => mod.GetBackgroundSlot("Backgrounds/SpiritBiomeSurfaceFar");

		public override int ChooseMiddleTexture() => mod.GetBackgroundSlot("Backgrounds/SpiritBiomeSurfaceMid");

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
		{
			scale *= .76f;
			b += 100;
			return mod.GetBackgroundSlot("Backgrounds/SpiritBiomeSurfaceClose");
		}

		public override bool ChooseBgStyle() => !Main.gameMenu && NPC.downedMechBossAny && Main.LocalPlayer.GetSpiritPlayer().ZoneSpirit;

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