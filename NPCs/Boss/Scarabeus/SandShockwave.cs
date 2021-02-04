using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	public class SandShockwave : ModProjectile
	{

		public override void SetStaticDefaults() => DisplayName.SetDefault("Sand");

		readonly int passivetime = 30;
		readonly int activetime = 40;
		Vector2 startingpoint;
		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = passivetime + activetime;
			projectile.hide = true;
			projectile.direction = Main.rand.NextBool() ? 1 : -1;
			projectile.tileCollide = false;
		}
		public override bool CanDamage() => projectile.ai[1] != 0;
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.Center.ToVector2() - targetHitbox.Size() / 2, targetHitbox.Size(), startingpoint, projectile.Center);
		public override void AI()
		{
			projectile.ai[0]++;
			if(projectile.ai[0] > passivetime && projectile.ai[1] == 0) {
				projectile.ai[1]++;
				Main.PlaySound(new LegacySoundStyle(SoundID.Item, 14).WithVolume(0.5f).WithPitchVariance(0.2f), projectile.Center);
				projectile.velocity.Y = -12;
				for(int i = 0; i < 3; i++) {
					Gore gore = Gore.NewGoreDirect(projectile.Center + Main.rand.NextVector2Square(-18, 18), Main.rand.NextVector2Circular(3, 3), GoreID.ChimneySmoke1);
					gore.timeLeft = 20;
				}
			}

			if (projectile.ai[1] == 0) {
				startingpoint = projectile.Center;
				Vector2 dustvel = -Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 8);
				for (int i = 0; i < 3; i++) {
					int d = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 32, dustvel.X, dustvel.Y);
					Main.dust[d].noGravity = true;
				}
			}
			else {
				projectile.hide = false;
				projectile.alpha = (activetime - projectile.timeLeft) * (255 / activetime);
				projectile.rotation += 0.1f + projectile.direction;
				if (Main.rand.Next(5) == 0)
					Gore.NewGorePerfect(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.Pi / 14) / 2, mod.GetGoreSlot("Gores/SandBall"), Main.rand.NextFloat(0.6f, 0.8f));
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2D = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(texture2D, projectile.Center - Main.screenPosition, null, lightColor * projectile.Opacity, projectile.rotation, texture2D.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}