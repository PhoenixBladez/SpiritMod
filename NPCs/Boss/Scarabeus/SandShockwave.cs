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
	public class SandShockwave : ModProjectile
	{

		public override void SetStaticDefaults() => DisplayName.SetDefault("Sand");

		readonly int passivetime = 40;
		readonly int activetime = 40;
		Vector2 startingpoint;
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = passivetime + activetime;
			Projectile.hide = true;
			Projectile.direction = Main.rand.NextBool() ? 1 : -1;
			Projectile.tileCollide = false;
		}
		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => Projectile.ai[1] != 0;
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.Center.ToVector2() - targetHitbox.Size() / 2,
																													 targetHitbox.Size(),
																													 startingpoint,
																													 Projectile.Center);
		public override void AI()
		{
			Projectile.ai[0]++;
			if(Projectile.ai[0] > passivetime && Projectile.ai[1] == 0) {
				Projectile.ai[1]++;
				SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.5f, PitchVariance = 0.2f }, Projectile.Center);
				Projectile.velocity.Y = -12;
				for(int i = 0; i < 3; i++) {
					Gore gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center + Main.rand.NextVector2Square(-18, 18), Main.rand.NextVector2Circular(3, 3), GoreID.ChimneySmoke1);
					gore.timeLeft = 20;
				}
			}

			if (Projectile.ai[1] == 0) {
				startingpoint = Projectile.Center;
				Vector2 dustvel = -Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 5) * 3;
				Gore gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center + Main.rand.NextVector2Square(-18, 18), Main.rand.NextVector2Circular(3, 3), GoreID.ChimneySmoke1, 0.6f);
				gore.timeLeft = 20;
				for (int i = 0; i < 4; i++) {
					Dust dust = Dust.NewDustPerfect(Projectile.Center + (Vector2.UnitY * 20), Mod.Find<ModDust>("SandDust").Type, dustvel);
					dust.position.X += Main.rand.NextFloat(-6, 6);
					dust.noGravity = false;
					dust.scale = 0.75f;
				}
			}
			else {
				Projectile.hide = false;
				Projectile.alpha = (activetime - Projectile.timeLeft) * (255 / activetime);
				Projectile.rotation += 0.1f + Projectile.direction;
				if (Main.rand.Next(4) == 0)
					Gore.NewGorePerfect(Projectile.GetSource_Death(), Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.Pi / 14) / 2, Mod.Find<ModGore>("Gores/SandBall").Type, Main.rand.NextFloat(0.6f, 0.8f));

				for (int i = 0; i < 3; i++) {
					Dust dust = Dust.NewDustPerfect(Projectile.Center + (Vector2.UnitY * 16), Mod.Find<ModDust>("SandDust").Type, Projectile.velocity.RotatedByRandom(MathHelper.Pi / 8) * 0.2f);
					dust.position += dust.velocity * Main.rand.NextFloat(20, 25);
					dust.noGravity = true;
					dust.scale = Main.rand.NextFloat(0.5f, 1.2f);
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
			Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition, null, lightColor * Projectile.Opacity, Projectile.rotation, texture2D.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}