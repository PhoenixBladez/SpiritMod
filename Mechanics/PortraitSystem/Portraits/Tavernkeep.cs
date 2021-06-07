using Microsoft.Xna.Framework;
using Terraria.ID;

namespace SpiritMod.Mechanics.PortraitSystem.Portraits
{
	public class Tavernkeep : BasePortrait
	{
		public override int ID => NPCID.DD2Bartender;

		public Tavernkeep() : base(null) { }

		public override Rectangle GetFrame(string speech)
		{
			if (speech == "Huh? How did I get here? The last thing I remember was a portal opening up in front of me...")
				return new Rectangle(110, 0, 108, 108);
			return base.GetFrame(speech);
		}
	}
}
