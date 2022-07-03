
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Infernon
{
	public class InfernalWave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernal Wave");
			Main.projFrames[Projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 55;

			Projectile.hostile = true;
			Projectile.tileCollide = false;

			Projectile.penetrate = -1;
			Projectile.extraUpdates = 1;
		}

		public override bool PreAI()
		{
			if (Projectile.localAI[1] == 0f) {
				Projectile.localAI[1] = 1f;
				SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
			}

			if (Projectile.ai[0] == 0f || Projectile.ai[0] == 2f) {
				Projectile.scale += 0.01f;
				Projectile.alpha -= 50;
				if (Projectile.alpha <= 0) {
					Projectile.ai[0] = 1f;
					Projectile.alpha = 0;
				}
			}
			else if (Projectile.ai[0] == 1f) {
				Projectile.scale -= 0.01f;
				Projectile.alpha += 50;
				if (Projectile.alpha >= 255) {
					Projectile.ai[0] = 2f;
					Projectile.alpha = 255;
				}
			}

			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 5)
				Projectile.frame = (Projectile.frame++) % Main.projFrames[Projectile.type];
			Projectile.rotation = Projectile.velocity.ToRotation() + 4.71F;
			return false;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(BuffID.OnFire, 300);
			else
				target.AddBuff(BuffID.OnFire, 600);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(200, 200, 200, Projectile.alpha);
		}

		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 0.9f;
			Main.dust[dust].scale = 0.9f;
		}
	}
}
