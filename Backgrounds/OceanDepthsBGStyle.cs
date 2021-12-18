using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Backgrounds
{
	public class OceanDepthsBGStyle : ModSurfaceBgStyle
	{
		public override int ChooseFarTexture() => mod.GetBackgroundSlot("Backgrounds/OceanUnderwaterBG3");

		public override int ChooseMiddleTexture() => mod.GetBackgroundSlot("Backgrounds/OceanUnderwaterBG2");

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
		{
			scale *= .86f;
			b -= 300;
			return mod.GetBackgroundSlot("Backgrounds/OceanUnderwaterBG1");
		}

		public override bool ChooseBgStyle() => !Main.gameMenu && Main.LocalPlayer.ZoneBeach && Main.LocalPlayer.GetSpiritPlayer().Submerged(20);

		// Use this to keep far Backgrounds like the mountains.
		public override void ModifyFarFades(float[] fades, float transitionSpeed)
		{
			for (int i = 0; i < fades.Length; i++)
			{
				if (i == Slot)
				{
					fades[i] += transitionSpeed;
					if (fades[i] > 1f)
						fades[i] = 1f;
				}
				else
				{
					fades[i] -= transitionSpeed;
					if (fades[i] < 0f)
						fades[i] = 0f;
				}
			}
		}
	}
}