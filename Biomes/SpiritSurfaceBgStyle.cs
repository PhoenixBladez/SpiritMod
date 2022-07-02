using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Biomes
{
	public class SpiritSurfaceBgStyle : ModSurfaceBackgroundStyle
	{
		public override int ChooseFarTexture() => BackgroundTextureLoader.GetBackgroundSlot(Mod, "Biomes/Assets/SpiritBiomeSurfaceFar");
		public override int ChooseMiddleTexture() => BackgroundTextureLoader.GetBackgroundSlot(Mod, "Biomes/Assets/SpiritBiomeSurfaceMid");
		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) => BackgroundTextureLoader.GetBackgroundSlot(Mod, "Biomes/Assets/SpiritBiomeSurfaceClose");

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