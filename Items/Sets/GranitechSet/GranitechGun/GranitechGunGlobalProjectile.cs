using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
	class GranitechGunGlobalProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

		public bool spawnedByGranitechGun = false;

		public override void Kill(Projectile projectile, int timeLeft) => spawnedByGranitechGun = false;

		public override bool PreAI(Projectile projectile)
		{
			//if (spawnedByGranitechGun && projectile.timeLeft % 8 == 0)
			//{
			//	const float SpeedMax = 0.6f;

			//	var vel = new Vector2(Main.rand.NextFloat(-SpeedMax, SpeedMax), Main.rand.NextFloat(-SpeedMax, SpeedMax));
			//	var d = Dust.NewDustPerfect(projectile.Center, ModContent.DustType<GranitechGunDust>(), vel);
			//	GranitechGunDust.SetFrame(d, 2);
			//}
			return true;
		}
	}
}
