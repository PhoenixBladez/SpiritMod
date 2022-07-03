using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.BowsMisc.OrnamentBow
{
	public class Ornament_Arrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ornament Arrow");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.arrow = true;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
		}

		public override void AI()
		{
			for (int i = 0; i < 1; i++)
			{
				int num = Projectile.frameCounter;
				Projectile.frameCounter = num + 1;
				Projectile.localAI[0] += 1f;
				for (int num41 = 0; num41 < 4; num41 = num + 1)
				{
					Vector2 value8 = -Vector2.UnitY.RotatedBy(Projectile.localAI[0] * 0.1308997f + num41 * MathHelper.Pi) * new Vector2(2f, 10f) - Projectile.rotation.ToRotationVector2();
					int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.RainbowTorch, 0f, 0f, 160, Main.DiscoColor, 1f);
					Main.dust[dust].scale = 0.7f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].position = Projectile.Center + value8 + Projectile.velocity * 2f;
					Main.dust[dust].velocity = Vector2.Normalize(Projectile.Center + Projectile.velocity * 2f - Main.dust[dust].position) / 8f + Projectile.velocity;
					num = num41;
				}
			}
			for (int i = 0; i < 1; i++)
			{
				int num = Projectile.frameCounter;
				Projectile.frameCounter = num + 1;
				Projectile.localAI[0] += 1f;
				for (int num41 = 0; num41 < 4; num41 = num + 1)
				{
					Vector2 value8 = -Vector2.UnitY.RotatedBy(Projectile.localAI[0] * 0.1308997f + num41 * 3.14159274f) * new Vector2(10f, 2f) - Projectile.rotation.ToRotationVector2();
					int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.RainbowTorch, 0f, 0f, 160, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
					Main.dust[dust].scale = 0.7f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].position = Projectile.Center + value8 + Projectile.velocity * 2f;
					Main.dust[dust].velocity = Vector2.Normalize(Projectile.Center + Projectile.velocity * 2f - Main.dust[dust].position) / 8f + Projectile.velocity;
					num = num41;
				}
			}

			Lighting.AddLight(Projectile.Center, new Vector3(Main.DiscoR, Main.DiscoG, Main.DiscoB) / 512f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects effect = Projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Color col = Lighting.GetColor((int)(Projectile.Center.Y) / 16, (int)(Projectile.Center.Y) / 16);
			var basePos = Projectile.Center - Main.screenPosition + new Vector2(0.0f, Projectile.gfxOffY);

			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			int height = texture.Height / Main.projFrames[Projectile.type];
			var frame = new Rectangle(0, height * Projectile.frame, texture.Width, height);
			Vector2 origin = frame.Size() / 2f;
			int reps = 1;
			while (reps < 5)
			{
				col = Projectile.GetAlpha(Color.Lerp(col, Color.White, 2.5f));
				float num7 = 5 - reps;
				Color drawCol = col * (num7 / (ProjectileID.Sets.TrailCacheLength[Projectile.type] * 1.5f));
				Vector2 oldPo = Projectile.oldPos[reps];
				float rotation = Projectile.rotation;
				SpriteEffects effects2 = effect;
				if (ProjectileID.Sets.TrailingMode[Projectile.type] == 2)
				{
					rotation = Projectile.oldRot[reps];
					effects2 = Projectile.oldSpriteDirection[reps] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				}
				Vector2 drawPos = oldPo + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
				Main.spriteBatch.Draw(texture, drawPos, frame, drawCol, rotation + Projectile.rotation * (reps - 1) * -effect.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin, MathHelper.Lerp(Projectile.scale, 3f, reps / 15f), effects2, 0.0f);
				reps++;
			}

			Main.spriteBatch.Draw(texture, basePos, frame, new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 175), Projectile.rotation, origin, Projectile.scale, effect, 0.0f);

			height = texture.Height / Main.projFrames[Projectile.type];
			frame = new Rectangle(0, height * Projectile.frame, texture.Width, height);
			origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float num99 = (float)(Math.Cos(Main.GlobalTimeWrappedHourly % 2.40000009536743 / 2.40000009536743 * MathHelper.TwoPi) / 4.0f + 0.5f);

			Color color2 = new Color(sbyte.MaxValue - Projectile.alpha, sbyte.MaxValue - Projectile.alpha, sbyte.MaxValue - Projectile.alpha, 0).MultiplyRGBA(Color.White);
			for (int i = 0; i < 4; ++i)
			{
				Color drawCol = Projectile.GetAlpha(color2) * (1f - num99);
				Vector2 offset = ((i / 4 * MathHelper.TwoPi) + Projectile.rotation).ToRotationVector2();
				Vector2 position2 = Projectile.Center + offset * (8.0f * num99 + 2.0f) - Main.screenPosition - texture.Size() * Projectile.scale / 2f + origin * Projectile.scale + new Vector2(0.0f, Projectile.gfxOffY);
				Main.spriteBatch.Draw(texture, position2, frame, drawCol, Projectile.rotation, origin, Projectile.scale, effect, 0.0f);
			}

			Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3() / 2f);
			return false;
		}

		private void SpawnArrows(IEntitySource src)
		{
			for (int index = 0; index < 10; ++index) {
				int i = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.RainbowTorch, 0.0f, 0.0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
				Main.dust[i].noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.Shatter with { Volume = 0.4f });

			Player player = Main.player[Projectile.owner];
			int extraArrows = Main.rand.Next(5);

			for (int i = 0; i < 1 + extraArrows; i++) {
				int arrowType = Main.rand.Next(6);
				int dustType = DustID.Stone;
				switch (arrowType) {
					case 0:
						arrowType = ModContent.ProjectileType<GemBows.Amethyst_Bow.Amethyst_Arrow>();
						dustType = DustID.GemAmethyst;
						break;
					case 1:
						arrowType = ModContent.ProjectileType<GemBows.Topaz_Bow.Topaz_Arrow>();
						dustType = DustID.GemTopaz;
						break;
					case 2:
						arrowType = ModContent.ProjectileType<GemBows.Sapphire_Bow.Sapphire_Arrow>();
						dustType = DustID.GemSapphire;
						break;
					case 3:
						arrowType = ModContent.ProjectileType<GemBows.Emerald_Bow.Emerald_Arrow>();
						dustType = DustID.GemEmerald;
						break;
					case 4:
						arrowType = ModContent.ProjectileType<GemBows.Ruby_Bow.Ruby_Arrow>();
						dustType = DustID.GemRuby;
						break;
					case 5:
						arrowType = ModContent.ProjectileType<GemBows.Diamond_Bow.Diamond_Arrow>();
						dustType = DustID.GemDiamond;
						break;
					default:
						break;
				}
				float x = Main.rand.Next(-80, 80);
				float y = Main.rand.Next(-60, -20);
				int a = Projectile.NewProjectile(src, player.Center.X + x, player.Center.Y + y, 0f, 0f, arrowType, (int)(player.GetDamage(DamageClass.Ranged).ApplyTo(player.HeldItem.damage * 0.7f)), 2f, player.whoAmI);
				Vector2 vector2_2 = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - Main.projectile[a].Center) * Main.rand.Next(14, 18);
				Main.projectile[a].velocity = vector2_2;
				for (int j = 0; j < 16f; ++j) {
					Vector2 v = (Vector2.UnitX * 0.1f + -Vector2.UnitY.RotatedBy(j * (MathHelper.TwoPi / 16f), new Vector2()) * new Vector2(4f)).RotatedBy(Projectile.velocity.ToRotation());
					int dust = Dust.NewDust(new Vector2(player.Center.X + x, player.Center.Y + y), 8, 8, dustType, 0.0f, 0.0f, 0, default, 1f);
					Main.dust[dust].scale = 0.9f;
					Main.dust[dust].alpha = 200;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = false;
					Main.dust[dust].position = new Vector2(player.Center.X + x, player.Center.Y + y) + v;
					Main.dust[dust].velocity = new Vector2(Projectile.velocity.X, Projectile.velocity.Y) * 0.0f + v.SafeNormalize(Vector2.UnitY) * 1f;
				}
			}
		}

		public override void Kill(int timeLeft) => SoundEngine.PlaySound(SoundID.Shatter with { Volume = 0.4f });

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => SpawnArrows(Projectile.GetSource_OnHit(target));

		public override void OnHitPvp(Player target, int damage, bool crit) => SpawnArrows(Projectile.GetSource_OnHit(target));
	}
}
