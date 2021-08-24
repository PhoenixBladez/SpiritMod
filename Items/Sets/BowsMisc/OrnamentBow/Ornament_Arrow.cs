using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BowsMisc.OrnamentBow
{
	public class Ornament_Arrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ornament Arrow");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 14; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.arrow = true;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.arrow = true;
		}

		public override void AI()
		{
			for (int i = 0; i < 1; i++)
			{
				int num = projectile.frameCounter;
				projectile.frameCounter = num + 1;
				projectile.localAI[0] += 1f;
				for (int num41 = 0; num41 < 4; num41 = num + 1)
				{
					Vector2 value8 = -Vector2.UnitY.RotatedBy(projectile.localAI[0] * 0.1308997f + num41 * MathHelper.Pi) * new Vector2(2f, 10f) - projectile.rotation.ToRotationVector2();
					int dust = Dust.NewDust(projectile.Center, 0, 0, DustID.Rainbow, 0f, 0f, 160, Main.DiscoColor, 1f);
					Main.dust[dust].scale = 0.7f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].position = projectile.Center + value8 + projectile.velocity * 2f;
					Main.dust[dust].velocity = Vector2.Normalize(projectile.Center + projectile.velocity * 2f - Main.dust[dust].position) / 8f + projectile.velocity;
					num = num41;
				}
			}
			for (int i = 0; i < 1; i++)
			{
				int num = projectile.frameCounter;
				projectile.frameCounter = num + 1;
				projectile.localAI[0] += 1f;
				for (int num41 = 0; num41 < 4; num41 = num + 1)
				{
					Vector2 value8 = -Vector2.UnitY.RotatedBy(projectile.localAI[0] * 0.1308997f + num41 * 3.14159274f) * new Vector2(10f, 2f) - projectile.rotation.ToRotationVector2();
					int dust = Dust.NewDust(projectile.Center, 0, 0, DustID.Rainbow, 0f, 0f, 160, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
					Main.dust[dust].scale = 0.7f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].position = projectile.Center + value8 + projectile.velocity * 2f;
					Main.dust[dust].velocity = Vector2.Normalize(projectile.Center + projectile.velocity * 2f - Main.dust[dust].position) / 8f + projectile.velocity;
					num = num41;
				}
			}

			Lighting.AddLight(projectile.Center, new Vector3(Main.DiscoR, Main.DiscoG, Main.DiscoB) / 512f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects effect = projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Color col = Lighting.GetColor((int)(projectile.Center.Y) / 16, (int)(projectile.Center.Y) / 16);
			var basePos = projectile.Center - Main.screenPosition + new Vector2(0.0f, projectile.gfxOffY);

			Texture2D texture = Main.projectileTexture[projectile.type];

			int height = texture.Height / Main.projFrames[projectile.type];
			var frame = new Rectangle(0, height * projectile.frame, texture.Width, height);
			Vector2 origin = frame.Size() / 2f;
			int reps = 1;
			while (reps < 5)
			{
				col = projectile.GetAlpha(Color.Lerp(col, Color.White, 2.5f));
				float num7 = 5 - reps;
				Color drawCol = col * (num7 / (ProjectileID.Sets.TrailCacheLength[projectile.type] * 1.5f));
				Vector2 oldPo = projectile.oldPos[reps];
				float rotation = projectile.rotation;
				SpriteEffects effects2 = effect;
				if (ProjectileID.Sets.TrailingMode[projectile.type] == 2)
				{
					rotation = projectile.oldRot[reps];
					effects2 = projectile.oldSpriteDirection[reps] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				}
				Vector2 drawPos = oldPo + projectile.Size / 2f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
				Main.spriteBatch.Draw(texture, drawPos, frame, drawCol, rotation + projectile.rotation * (reps - 1) * -effect.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin, MathHelper.Lerp(projectile.scale, 3f, reps / 15f), effects2, 0.0f);
				reps++;
			}

			Main.spriteBatch.Draw(texture, basePos, frame, new Color(255 - projectile.alpha, 255 - projectile.alpha, 255 - projectile.alpha, 175), projectile.rotation, origin, projectile.scale, effect, 0.0f);

			height = texture.Height / Main.projFrames[projectile.type];
			frame = new Rectangle(0, height * projectile.frame, texture.Width, height);
			origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float num99 = (float)(Math.Cos(Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * MathHelper.TwoPi) / 4.0f + 0.5f);

			Color color2 = new Color(sbyte.MaxValue - projectile.alpha, sbyte.MaxValue - projectile.alpha, sbyte.MaxValue - projectile.alpha, 0).MultiplyRGBA(Color.White);
			for (int i = 0; i < 4; ++i)
			{
				Color drawCol = projectile.GetAlpha(color2) * (1f - num99);
				Vector2 offset = ((i / 4 * MathHelper.TwoPi) + projectile.rotation).ToRotationVector2();
				Vector2 position2 = projectile.Center + offset * (8.0f * num99 + 2.0f) - Main.screenPosition - texture.Size() * projectile.scale / 2f + origin * projectile.scale + new Vector2(0.0f, projectile.gfxOffY);
				Main.spriteBatch.Draw(texture, position2, frame, drawCol, projectile.rotation, origin, projectile.scale, effect, 0.0f);
			}

			Lighting.AddLight(projectile.Center, Color.Purple.ToVector3() / 2f);
			return false;
		}

		private void SpawnArrows()
		{
			for (int index = 0; index < 10; ++index) {
				int i = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Rainbow, 0.0f, 0.0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
				Main.dust[i].noGravity = true;
			}
			Main.PlaySound(SoundID.Trackable, (int)projectile.position.X, (int)projectile.position.Y, 193, 1f, -0.2f);

			Player player = Main.player[projectile.owner];
			int extraArrows = Main.rand.Next(5);

			for (int i = 0; i < 1 + extraArrows; i++) {
				int arrowType = Main.rand.Next(6);
				int dustType = DustID.Stone;
				switch (arrowType) {
					case 0:
						arrowType = ModContent.ProjectileType<GemBows.Amethyst_Bow.Amethyst_Arrow>();
						dustType = DustID.AmethystBolt;
						break;
					case 1:
						arrowType = ModContent.ProjectileType<GemBows.Topaz_Bow.Topaz_Arrow>();
						dustType = DustID.TopazBolt;
						break;
					case 2:
						arrowType = ModContent.ProjectileType<GemBows.Sapphire_Bow.Sapphire_Arrow>();
						dustType = DustID.SapphireBolt;
						break;
					case 3:
						arrowType = ModContent.ProjectileType<GemBows.Emerald_Bow.Emerald_Arrow>();
						dustType = DustID.EmeraldBolt;
						break;
					case 4:
						arrowType = ModContent.ProjectileType<GemBows.Ruby_Bow.Ruby_Arrow>();
						dustType = DustID.RubyBolt;
						break;
					case 5:
						arrowType = ModContent.ProjectileType<GemBows.Diamond_Bow.Diamond_Arrow>();
						dustType = DustID.DiamondBolt;
						break;
					default:
						break;
				}
				float x = Main.rand.Next(-80, 80);
				float y = Main.rand.Next(-60, -20);
				int a = Projectile.NewProjectile(player.Center.X + x, player.Center.Y + y, 0f, 0f, arrowType, (int)(player.HeldItem.damage * 0.7f * player.rangedDamage), 2f, player.whoAmI);
				Vector2 vector2_2 = Vector2.Normalize(new Vector2(projectile.Center.X, projectile.Center.Y) - Main.projectile[a].Center) * Main.rand.Next(14, 18);
				Main.projectile[a].velocity = vector2_2;
				for (int j = 0; j < 16f; ++j) {
					Vector2 v = (Vector2.UnitX * 0.1f + -Vector2.UnitY.RotatedBy(j * (MathHelper.TwoPi / 16f), new Vector2()) * new Vector2(4f)).RotatedBy(projectile.velocity.ToRotation());
					int dust = Dust.NewDust(new Vector2(player.Center.X + x, player.Center.Y + y), 8, 8, dustType, 0.0f, 0.0f, 0, default, 1f);
					Main.dust[dust].scale = 0.9f;
					Main.dust[dust].alpha = 200;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = false;
					Main.dust[dust].position = new Vector2(player.Center.X + x, player.Center.Y + y) + v;
					Main.dust[dust].velocity = new Vector2(projectile.velocity.X, projectile.velocity.Y) * 0.0f + v.SafeNormalize(Vector2.UnitY) * 1f;
				}
			}
		}

		public override void Kill(int timeLeft) => Main.PlaySound(SoundID.Trackable, (int)projectile.position.X, (int)projectile.position.Y, 193, 1f, -0.2f);

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => SpawnArrows();

		public override void OnHitPvp(Player target, int damage, bool crit) => SpawnArrows();
	}
}
