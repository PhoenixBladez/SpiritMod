using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Warden
{
	class VoidGlobalProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

		public bool hasTeleported = false;
		public int teleportWhoAmI = -1;

		public override bool PreAI(Projectile projectile)
		{
			if (hasTeleported && teleportWhoAmI != -1)
			{
				Microsoft.Xna.Framework.Rectangle otherBox = Main.projectile[teleportWhoAmI].Hitbox;
				otherBox.Inflate(10, 10);

				if (!projectile.Hitbox.Intersects(otherBox))
				{
					hasTeleported = false;
					teleportWhoAmI = -1;
				}
			}
			return true;
		}
	}
}
