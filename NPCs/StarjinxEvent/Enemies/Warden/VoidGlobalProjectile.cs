using SpiritMod.NPCs.StarjinxEvent.Enemies.Warden.Projectiles;
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

		public void OnTeleport(Projectile projectile)
		{
			if (projectile.type == ModContent.ProjectileType<VoidProjectile>())
			{
				int plr = Player.FindClosest(projectile.position, projectile.width, projectile.height);
				if (plr != -1)
				{
					Player player = Main.player[plr];
					if (player.active && !player.dead)
						projectile.velocity = projectile.DirectionTo(player.Center) * projectile.velocity.Length();
				}
			}
		}
	}
}
