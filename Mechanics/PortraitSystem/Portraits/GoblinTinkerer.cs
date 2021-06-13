using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Mechanics.PortraitSystem.Portraits
{
	class GoblinTinkerer : BasePortrait
	{
		public override int ID => NPCID.GoblinTinkerer;

		public GoblinTinkerer() : base(null) { }

		public override Rectangle GetFrame(string speech, NPC npc)
		{
			if (speech == "Thank you for freeing me, human.  I was tied up and left here by the other goblins.  You could say that we didn't get along very well.")
				return new Rectangle(110, 0, 108, 108);
			return base.GetFrame(speech, npc);
		}
	}
}
