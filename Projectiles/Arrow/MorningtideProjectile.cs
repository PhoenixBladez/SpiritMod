using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Projectiles.Arrow
{
	public class MorningtideProjectile : ModProjectile, IDrawAdditive, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dawnstrike Shafts");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.alpha = 100;
			Projectile.timeLeft = 200;
			Projectile.height = 50;
			Projectile.width = 20;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(Projectile, new GradientTrail(new Color(255, 225, 117), new Color(91, 21, 150)), new RoundCap(), new SleepingStarTrailPosition(), 100f, 180f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_4").Value, 0.01f, 1f, 1f));
			tM.CreateTrail(Projectile, new GradientTrail(new Color(255, 225, 117), new Color(91, 21, 150)), new RoundCap(), new SleepingStarTrailPosition(), 90f, 180f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_1").Value, 0.01f, 1f, 1f));
			tM.CreateTrail(Projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new SleepingStarTrailPosition(), 12f, 80f, new DefaultShader());
		}

		float num;

		public override void AI()
		{
			if (Projectile.timeLeft >= 199)
			{
				Projectile.alpha = 255;
				Projectile.height = 20;
				Projectile.width = 10;
			}
			else
			{
				Lighting.AddLight(Projectile.position, 0.205f * 1.85f, 0.135f * 1.85f, 0.255f * 1.85f);
				Projectile.alpha = 0;
				Projectile.height = 50;
				Projectile.width = 26;
			}
			num += .4f;
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Vector2 mouse = Main.MouseWorld;
			int amount = Main.rand.Next(1, 3);
			for (int i = 0; i < amount; ++i)
			{
				Vector2 pos = new Vector2(mouse.X + target.width * 0.5f + Main.rand.Next(-35, 36), mouse.Y - 220f);
				pos.X = (pos.X * 10f + mouse.X) / 11f - 350;
				pos.Y -= Main.rand.Next(-20, 20);
				float spX = mouse.X + target.width * 0.5f + Main.rand.Next(-100, 101) - mouse.X;
				float spY = mouse.Y - pos.Y;
				if (spY < 0f)
					spY *= -1f;
				if (spY < 20f)
					spY = 20f;

				float length = (float)Math.Sqrt((double)(spX * spX + spY * spY));
				length = 12 / length;
				spX *= length;
				spY *= length;
				spX = spX + (float)Main.rand.Next(-40, 41) * 0.02f;
				spY = spY + (float)Main.rand.Next(-40, 41) * 0.12f;
				spX *= (float)Main.rand.Next(75, 150) * 0.006f;
				pos.X += (float)Main.rand.Next(-20, 21);
				DustHelper.DrawCircle(new Vector2(pos.X, pos.Y), 222, 1, 1f, 1.35f, .85f, .85f);

				Projectile.NewProjectile(Projectile.GetSource_OnHit(target), pos.X, pos.Y, spX * Projectile.direction, spY, ModContent.ProjectileType<MorningtideProjectile2>(), Projectile.damage / 3 * 2, 2, Main.player[Projectile.owner].whoAmI);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.timeLeft < 196)
			{
				Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				}
			}
			else
				return false;
			return true;
		}
		public void AdditiveCall(SpriteBatch spriteBatch, Vector2 screenPos)
		{
			if (Projectile.timeLeft < 196)
			{
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					Color color = new Color(255, 255, 200) * 0.75f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

					float scale = Projectile.scale;
					Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Projectiles/Arrow/Morningtide_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

					spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - screenPos, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);

					Color color1 = new Color(255, 186, 252) * 0.45475f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - screenPos + new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3)), null, color1, Projectile.rotation, tex.Size() / 2, scale * 1.5425f, default, default);
				}
			}
		}

		public override Color? GetAlpha(Color lightColor) => Color.Orange;

		public override void Kill(int timLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.Center);

			for (int k = 0; k < 18; k++)
			{
				var d = Dust.NewDustPerfect(Projectile.Center, 222, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.75f);
				d.noGravity = true;

				var d1 = Dust.NewDustPerfect(Projectile.Center, 222, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.75f);
				d1.shader = GameShaders.Armor.GetSecondaryShader(100, Main.LocalPlayer);
			}
		}
	}
}
