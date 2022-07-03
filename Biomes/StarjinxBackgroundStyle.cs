using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Biomes
{
	/// <summary>
	/// Used solely to disable vanilla background drawing during the Starjinx event. 
	/// </summary>
	public class StarjinxBackgroundStyle : ModSurfaceBackgroundStyle
	{
		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) => mod.GetBackgroundSlot("Backgrounds/Assets/emptyBG");

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