using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Backgrounds
{
	public class OceanDepthsBGStyle : ModSurfaceBackgroundStyle
	{
		public override int ChooseFarTexture() => BackgroundTextureLoader.GetBackgroundSlot(Mod, "Biomes/Assets/OceanUnderwaterBG3");
		public override int ChooseMiddleTexture() => BackgroundTextureLoader.GetBackgroundSlot(Mod, "Biomes/Assets/OceanUnderwaterBG2");

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
		{
			scale *= .86f;
			b -= 300;
			return BackgroundTextureLoader.GetBackgroundSlot("Biomes/Assets/OceanUnderwaterBG2");
		}

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