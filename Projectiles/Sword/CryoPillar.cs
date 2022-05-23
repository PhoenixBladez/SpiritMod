using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.Projectiles.Sword
{
	public class CryoPillar : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryo Pillar");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.damage = 1;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.timeLeft = 9999;
			projectile.tileCollide = true;
			projectile.extraUpdates = 1;
		}

		private void Trail(Vector2 from, Vector2 to)
		{
			float distance = Vector2.Distance(from, to);
			float step = 1 / distance;
			for (float w = 0; w < distance; w += 4) {
				Dust.NewDustPerfect(Vector2.Lerp(from, to, w * step), 39, Vector2.Zero).noGravity = true;
			}
		}
		//projectile.ai[0]: how many more pillars. Each one is one less
		//projectile.ai[1]: 0: center, 1: going left, 2: going right
		bool activated = false;
		float startposY = 0;
		public override bool PreAI()
		{
			if (startposY == 0) {
				startposY = projectile.position.Y;
			}
			projectile.velocity.X = 0;
			if (!activated) {
				projectile.velocity.Y = 24;
			}
			else {
				projectile.velocity.Y = -6;
				for (int i = 0; i < 10; i++) {
					int dust = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.BlueCrystalShard);
					Main.dust[dust].velocity = Vector2.Zero;
					Main.dust[dust].noGravity = true;
				}
				if (projectile.timeLeft == 10 && projectile.ai[0] > 0) {
					if (projectile.ai[1] == 1 || projectile.ai[1] == 0) {
						Projectile.NewProjectile(projectile.Center.X - projectile.width, startposY, 0, 0, ModContent.ProjectileType<CryoPillar>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0] - 1, 1);
					}
					if (projectile.ai[1] == 2 || projectile.ai[1] == 0) {
						Projectile.NewProjectile(projectile.Center.X + projectile.width, startposY, 0, 0, ModContent.ProjectileType<CryoPillar>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0] - 1, 2);
					}
				}
			}
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(ModContent.BuffType<CryoCrush>(), 180);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.Y != projectile.velocity.Y && !activated) {
				startposY = projectile.position.Y;
				projectile.velocity.Y = -6;
				activated = true;
				projectile.timeLeft = 30;
			}
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}

	}
}