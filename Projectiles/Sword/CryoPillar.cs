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
			Projectile.hostile = false;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.damage = 1;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 9999;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
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
				startposY = Projectile.position.Y;
			}
			Projectile.velocity.X = 0;
			if (!activated) {
				Projectile.velocity.Y = 24;
			}
			else {
				Projectile.velocity.Y = -6;
				for (int i = 0; i < 10; i++) {
					int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.BlueCrystalShard);
					Main.dust[dust].velocity = Vector2.Zero;
					Main.dust[dust].noGravity = true;
				}
				if (Projectile.timeLeft == 10 && Projectile.ai[0] > 0) {
					if (Projectile.ai[1] == 1 || Projectile.ai[1] == 0) {
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X - Projectile.width, startposY, 0, 0, ModContent.ProjectileType<CryoPillar>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0] - 1, 1);
					}
					if (Projectile.ai[1] == 2 || Projectile.ai[1] == 0) {
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X + Projectile.width, startposY, 0, 0, ModContent.ProjectileType<CryoPillar>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0] - 1, 2);
					}
				}
			}
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(5))
				target.AddBuff(ModContent.BuffType<CryoCrush>(), 180);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.Y != Projectile.velocity.Y && !activated) {
				startposY = Projectile.position.Y;
				Projectile.velocity.Y = -6;
				activated = true;
				Projectile.timeLeft = 30;
			}
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}

	}
}