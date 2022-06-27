using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.CollideableNPC
{
	public class CollideableNPCPlayer : ModPlayer
	{
		public int platformWhoAmI = -1;
		public int platformTimer = 0;
		public int platformDropTimer = 0;

		public NPC Platform => Main.npc[platformWhoAmI];

		public override void PreUpdateMovement()
		{
			if (platformWhoAmI != -1)
				Player.position.X += Platform.velocity.X;
		}
	}
}
