using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Flail
{
	public class GraniteMaceProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Colonnade");
		}

		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 34;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
		}

		public override bool PreAI()
		{
			Vector2 position = projectile.Center + Vector2.Normalize(projectile.velocity) * 10;

			Dust newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Electric, 0f, 0f, 0, default, .31f)];
			newDust.position = position;
			newDust.velocity = projectile.velocity.RotatedBy(Math.PI / 2, default) * 0.33F + projectile.velocity / 4;
			newDust.position += projectile.velocity.RotatedBy(Math.PI / 2, default);
			newDust.fadeIn = 0.5f;
			newDust.noGravity = true;
			newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Electric, 0f, 0f, 0, default, .31f)];
			newDust.position = position;
			newDust.velocity = projectile.velocity.RotatedBy(-Math.PI / 2, default) * 0.33F + projectile.velocity / 4;
			newDust.position += projectile.velocity.RotatedBy(-Math.PI / 2, default);
			newDust.fadeIn = 0.5F;
			newDust.noGravity = true;
			ProjectileExtras.FlailAI(projectile.whoAmI);
			return false;
		}
		public static void DrawChain(int index, Vector2 to, string chainPath)
		{
			Texture2D texture = ModContent.GetTexture(chainPath);
			Projectile projectile = Main.projectile[index];
			Vector2 vector = projectile.Center;
			Rectangle? sourceRectangle = null;
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
			float num = (float)texture.Height;
			Vector2 vector2 = to - vector;
			float rotation = (float)Math.Atan2((double)vector2.Y, (double)vector2.X) - 1.57f;
			bool flag = true;
			if (float.IsNaN(vector.X) && float.IsNaN(vector.Y)) {
				flag = false;
			}
			if (float.IsNaN(vector2.X) && float.IsNaN(vector2.Y)) {
				flag = false;
			}

			while (flag) {
				if ((double)vector2.Length() < (double)num + 1.0) {
					flag = false;
				}
				else {
					Vector2 value = vector2;
					value.Normalize();
					vector += value * num;
					vector2 = to - vector;
					Color color = Color.White;
					color = Color.White;
					Main.spriteBatch.Draw(texture, vector - Main.screenPosition, sourceRectangle, color, rotation, origin, 1f, SpriteEffects.None, 0f);
				}
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return ProjectileExtras.FlailTileCollide(projectile.whoAmI, oldVelocity);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter,
				"SpiritMod/Projectiles/Flail/GraniteMace_Chain");

			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (target.life <= 0) {
				if (projectile.friendly && !projectile.hostile) {
					ProjectileExtras.Explode(projectile.whoAmI, 30, 30,
					delegate {
					});

				}
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 109));
				{
					for (int i = 0; i < 20; i++) {
						int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Electric, 0f, -2f, 0, default, 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if (Main.dust[num].position != projectile.Center)
							Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
				int proj = Projectile.NewProjectile(target.Center.X, target.Center.Y,
					0, 0, mod.ProjectileType("GraniteSpike1"), projectile.damage / 2, projectile.knockBack, projectile.owner);
				Main.projectile[proj].timeLeft = 2;
			}
		}

	}
}
