using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	public class ScarabSandball : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Ball");
		}

		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 360;
			Projectile.scale = 0.75f;
		}

		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			for (int i = 0; i < 10; i++) {
				int d = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Sand, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
				Main.dust[d].noGravity = true;
				Main.dust[d].scale = 1.2f;
			}
			SoundEngine.PlaySound(SoundID.Dig, (int)Projectile.Center.X, (int)Projectile.Center.Y);
		}

		public override void AI()
		{
			Projectile.tileCollide = (Projectile.position.Y >= Projectile.ai[1]);

			Projectile.rotation += 0.1f;
			for (int i = -2; i < 2; i++) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center + 2 * Projectile.velocity, Mod.Find<ModDust>("SandDust").Type, Vector2.Normalize(Projectile.velocity).RotatedBy(Math.Sign(i) * MathHelper.Pi / 4) * Math.Abs(i));
				dust.noGravity = true;
				dust.scale = 0.65f;
			}

			if (Projectile.ai[0] == 0 && Projectile.velocity.Y < 15f) {
				Projectile.velocity.Y += 0.2f;
			}

			if (Projectile.ai[0] == 1) {
				if(Projectile.velocity.Y < 0) {
					Projectile.velocity.Y += 0.01f;
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 0.05f);
				}
				else {
					Projectile.velocity.X = 0;
					Projectile.ai[1]++;
					if (Projectile.ai[1] > 15)
						Projectile.velocity.Y += 0.4f;
					else
						Projectile.velocity.Y = 0f;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
			Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture2D.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
				return false;
		}
	}
}