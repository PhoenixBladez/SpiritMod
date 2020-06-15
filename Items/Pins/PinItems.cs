using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Pins
{
	// Want to add another map pin?
	// Follow the pattern here, and add the corresponding textures.
	// Careful with removing them, though - placed pins remain in the save data

	public class PinRed : AMapPin
	{
		public override string PinName => "Red";
		public override Color TextColor => Color.IndianRed;
	}

	public class PinGreen : AMapPin
	{
		public override string PinName => "Green";
		public override Color TextColor => Color.Lime;
	}

	public class PinBlue : AMapPin
	{
		public override string PinName => "Blue";
		public override Color TextColor => Color.DeepSkyBlue;
	}

	public class PinYellow : AMapPin
	{
		public override string PinName => "Yellow";
		public override Color TextColor => Color.Gold;
	}
}