using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.NPCs.Occultist
{
	public class OccultistHand : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Grasp");
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 48;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 10;
			projectile.timeLeft = 1000;
			projectile.tileCollide = true;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
		}
		public override void AI()
		{
			Vector2 position = projectile.Center + Vector2.Normalize(projectile.velocity) * 10;
			projectile.velocity.X *= 1.005f;
			Dust newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 231, 0f, 0f, 0, default(Color), .8f)];
			newDust.position = position;
			newDust.velocity = projectile.velocity.RotatedBy(Math.PI / 2, default(Vector2)) * 0.33F + projectile.velocity / 4;
			newDust.position += projectile.velocity.RotatedBy(Math.PI / 2, default(Vector2));
			newDust.fadeIn = 0.5f;
			newDust.noGravity = true;
			newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 231, 0f, 0f, 0, default(Color), .8f)];
			newDust.position = position;
			newDust.velocity = projectile.velocity.RotatedBy(-Math.PI / 2, default(Vector2)) * 0.33F + projectile.velocity / 4;
			newDust.position += projectile.velocity.RotatedBy(-Math.PI / 2, default(Vector2));
			newDust.fadeIn = 0.5F;
			newDust.noGravity = true;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Obstructed, 120, true);
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1);
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 12f;
			for (int num257 = 0; num257 < 20; num257++) {
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 231, 0f, 0f, 0, default(Color), 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
		}
	}
}