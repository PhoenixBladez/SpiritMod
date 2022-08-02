using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ModLoader;

namespace SpiritMod.Utilities.PhaseIndicatorCompat
{
	[AttributeUsage(AttributeTargets.Class)]
	internal class PhaseIndicatorAttribute : Attribute
	{
		internal float[] Phases;
		internal Texture2D Indicator;

		public PhaseIndicatorAttribute(string indicatorPath, params float[] phases)
		{
			Phases = phases;

			if (indicatorPath is not null)
				Indicator = ModContent.Request<Texture2D>(indicatorPath).Value;
		}
	}
}
