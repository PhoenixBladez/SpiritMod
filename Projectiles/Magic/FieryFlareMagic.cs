using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Prim;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class FieryFlareMagic : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slag Flare");
		}

		Vector2 startingpoint;
		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			aiType = -1;
			projectile.alpha = 255;
			projectile.timeLeft = 120;
			projectile.penetrate = 3;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.extraUpdates = 3;
		}
		public override void SendExtraAI(BinaryWriter writer) => writer.WriteVector2(startingpoint);
		public override void ReceiveExtraAI(BinaryReader reader) => startingpoint = reader.ReadVector2();
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.Center.ToVector2() - targetHitbox.Size() / 2,
																													 targetHitbox.Size(),
																													 startingpoint,
																													 projectile.Center);
		public override void AI()
		{
			if(projectile.ai[0] == 0) {
				startingpoint = projectile.Center;
				projectile.ai[0]++;
				projectile.netUpdate = true;
				SpiritMod.primitives.CreateTrail(new PrimFireTrail(projectile, new Color(255, 170, 0), 24, 20));
			}
			if (projectile.wet)
				projectile.Kill();
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int j = 0; j < 14; j++) {
				int dust = Dust.NewDust(target.Center, 0, 0, 6);
				Main.dust[dust].velocity = projectile.velocity.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(1.5f, 2.8f);
				Main.dust[dust].position += new Vector2(Main.rand.Next(-25, 26), Main.rand.Next(-10, 11)).RotatedBy(Main.dust[dust].velocity.ToRotation());
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(1.6f, 2f);
			}
			if (Main.rand.Next(6) == 2)
				target.AddBuff(BuffID.OnFire, 180);
			if (Main.rand.Next(4) == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
				Main.PlaySound(SoundID.Item74, target.Center);
				int n = 4;
				int deviation = Main.rand.Next(0, 300);
				for (int i = 0; i < n; i++) {
					float rotation = MathHelper.ToRadians(270 / n * i + deviation);
					Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
					perturbedSpeed.Normalize();
					perturbedSpeed.X *= 2.5f;
					perturbedSpeed.Y *= 2.5f;
					Projectile.NewProjectile(target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.Spark, projectile.damage / 2, 2, projectile.owner);
				}
			}
		}
	}
}
