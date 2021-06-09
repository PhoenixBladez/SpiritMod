using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace SpiritMod.Mechanics.PortraitSystem.Portraits
{
	public class OldMan : BasePortrait
	{
		public override int ID => NPCID.OldMan;

		public OldMan() : base(null) { }

		public override Rectangle GetFrame(string speech, NPC npc)
		{
			if (!Main.dayTime)
				return new Rectangle(110, 0, 108, 108);
			return base.GetFrame(speech, npc);
		}
	}
}
