using Microsoft.Xna.Framework;
using SpiritMod.Prim;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Projectiles.Magic
{
	public class FieryFlareMagic : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Slag Flare");

		Vector2 startingvel;
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			AIType = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 120;
			Projectile.penetrate = 3;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 3;
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(Projectile, new StandardColorTrail(new Color(255, 170, 0)), new RoundCap(), new DefaultTrailPosition(), 32f, 50f, new DefaultShader());

		public override void SendExtraAI(BinaryWriter writer) => writer.WriteVector2(startingvel);
		public override void ReceiveExtraAI(BinaryReader reader) => startingvel = reader.ReadVector2();
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, Color.OrangeRed.ToVector3());
			if (Projectile.ai[0] == 0) {
				Projectile.ai[0] = Main.rand.NextBool() ? -1 : 1;
				Projectile.ai[0] *= Main.rand.NextFloat(0.01f, 1.2f);
				startingvel = Projectile.velocity;
				Projectile.netUpdate = true;
				SpiritMod.primitives.CreateTrail(new PrimFireTrail(Projectile, new Color(255, 170, 0), 24, 12));
			}
			Projectile.ai[1]++;
			Projectile.velocity = startingvel.RotatedBy(Projectile.ai[0] * Math.Abs(Projectile.ai[1] - 60)/60 * MathHelper.Pi / 9);
			if (Projectile.wet)
				Projectile.Kill();
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int j = 0; j < 14; j++) {
				int dust = Dust.NewDust(target.Center, 0, 0, DustID.Torch);
				Main.dust[dust].velocity = Projectile.velocity.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(1.5f, 2.8f);
				Main.dust[dust].position += new Vector2(Main.rand.Next(-25, 26), Main.rand.Next(-10, 11)).RotatedBy(Main.dust[dust].velocity.ToRotation());
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(1.6f, 2f);
			}
			if (Main.rand.NextBool(6))
				target.AddBuff(BuffID.OnFire, 180);
			if (crit && Main.netMode != NetmodeID.MultiplayerClient) {
				SoundEngine.PlaySound(SoundID.Item74, target.Center);
				int n = 4;
				int deviation = Main.rand.Next(0, 300);
				for (int i = 0; i < n; i++) {
					float rotation = MathHelper.ToRadians(270 / n * i + deviation);
					Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
					perturbedSpeed.Normalize();
					perturbedSpeed.X *= 2.5f;
					perturbedSpeed.Y *= 2.5f;
					Projectile.NewProjectile(target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.Spark, Projectile.damage / 2, 2, Projectile.owner);
				}
			}
		}
	}
}
