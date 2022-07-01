using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.PlagueDoctor
{
	public class ToxicFlaskHostile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plague Vial");
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 14;

			Projectile.aiStyle = 2;
			Projectile.damage = 10;
			Projectile.friendly = false;
			Projectile.hostile = true;

			Projectile.alpha = 0;
		}

		public override bool PreAI()
		{
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
			foreach (var proj in list)
				if (Main.player[Projectile.owner].channel)
					if (Projectile != proj && proj.friendly)
						Projectile.Kill();
			return true;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 107);
			for (int i = 0; i < 30; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, 0f, -2f, 0, default, 1.2f);
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X += (Main.rand.Next(-30, 31) / 20 - 1.5f);
				dust.position.Y += (Main.rand.Next(-30, 31) / 20 - 1.5f);
				if (Main.dust[num].position != Projectile.Center) {
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
			int n = 2;
			int deviation = Main.rand.Next(0, 300);
			for (int i = 0; i < n; i++) {
				float rotation = MathHelper.ToRadians(270 / n * i + deviation);
				Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
				perturbedSpeed.Normalize();
				perturbedSpeed.X *= Main.rand.NextFloat(1.5f, 3.5f);
				perturbedSpeed.Y *= Main.rand.NextFloat(1.5f, 3.5f);
				int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.CursedDartFlame, Projectile.damage, 2, Projectile.owner);
				Main.projectile[p].hostile = true;
				Main.projectile[p].friendly = false;
			}
		}
	}
}
