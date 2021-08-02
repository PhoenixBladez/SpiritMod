using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.Sagittarius
{
	public class SagittariusConstellation : ModProjectile, IDrawAdditive
	{
		public override string Texture => "SpiritMod/Effects/Masks/Star";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Constellation");

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.timeLeft = 600;
			projectile.scale = Main.rand.NextFloat(0.12f, 0.2f);
			projectile.alpha = 255;
			projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			projectile.hide = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override bool CanDamage() => false;

		private Vector2 _position;

		private Player Owner => Main.player[projectile.owner];

		private ref float StarsLeft => ref projectile.ai[0];

		private ref float LastStar => ref projectile.ai[1];

		private ref float Timer => ref projectile.localAI[1];

		public override bool PreAI()
		{
			if (projectile.localAI[0] == 0)
			{
				_position = projectile.Center - Owner.MountedCenter;
				projectile.localAI[0]++;
				projectile.netUpdate = true;
			}

			projectile.Center = Owner.MountedCenter + _position;
			return true;
		}

		public override void AI()
		{
			projectile.rotation += 0.05f * Owner.direction * ((StarsLeft % 2 == 0) ? 1 : -1);
			if (++Timer < 20)
			{
				projectile.scale *= 1.05f;
				projectile.alpha = Math.Max(projectile.alpha - 10, 0);
			}

			if(Timer == 30 && !Main.dedServ)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithPitchVariance(0.2f).WithVolume(0.5f), projectile.Center);
				for (int i = 0; i < 3; i++)
					Particles.ParticleHandler.SpawnParticle(new Particles.StarParticle(projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.5f, 1.5f), Color.White * 0.7f, Main.rand.NextFloat(0.2f, 0.3f), 20));
			}

			if (Owner == Main.LocalPlayer)
			{
				if (Timer == 10 && StarsLeft > 1)
				{
					Projectile.NewProjectileDirect(Owner.MountedCenter - Owner.DirectionTo(Main.MouseWorld).RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(70, 150),
						Vector2.Zero, ModContent.ProjectileType<SagittariusConstellation>(), projectile.damage, projectile.knockBack, projectile.owner, StarsLeft - 1, projectile.whoAmI).netUpdate = true;
					projectile.netUpdate = true;
				}

				if (Timer == 30)
				{
					Projectile proj = Projectile.NewProjectileDirect(projectile.Center, projectile.DirectionTo(Main.MouseWorld).RotatedByRandom(MathHelper.PiOver2) * 18, 
						ModContent.ProjectileType<SagittariusConstellationArrow>(), projectile.damage, projectile.knockBack, projectile.owner, StarsLeft % 2);

					if (proj.modProjectile is SagittariusConstellationArrow)
						(proj.modProjectile as SagittariusConstellationArrow).TargetPos = Main.MouseWorld; 

					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);

					projectile.netUpdate = true;
				}
			}

			if (Timer > 50)
			{
				projectile.scale *= 0.98f;
				projectile.alpha += 10;
				if (projectile.alpha >= 255)
					projectile.Kill();
			}
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D MainTex = Main.projectileTexture[projectile.type];
			Texture2D Bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			Color color = (StarsLeft % 2 == 1) ? new Color(101, 255, 245) : new Color(255, 176, 244);
			float bloomopacity = 0.75f;
			float bloomscale = 0.75f;
			if (Timer > 15 && Timer < 45)
			{
				color = Color.Lerp(color, Color.White, 1 - Math.Abs(30 - Timer) / 15);
				bloomopacity = MathHelper.Lerp(bloomopacity, 2f, 1 - Math.Abs(30 - Timer) / 15);
				bloomscale = MathHelper.Lerp(bloomscale, 0.25f, 1 - Math.Abs(30 - Timer) / 15);
			}

			if (LastStar >= 0)
			{
				Projectile laststar = Main.projectile[(int)LastStar];
				if (laststar.active && laststar.type == projectile.type && laststar.ai[0] == projectile.ai[0] + 1 && laststar.owner == projectile.owner)
				{
					Texture2D Beam = mod.GetTexture("Textures/Trails/Trail_4");
					Vector2 scale = new Vector2(projectile.Distance(laststar.Center) / Beam.Width, projectile.scale * 20 / Beam.Height);
					float opacity = 0.8f * projectile.Opacity * laststar.Opacity;
					Vector2 origin = new Vector2(0, Beam.Height / 2);
					spriteBatch.Draw(Beam, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.White, color, 0.5f) * opacity, projectile.AngleTo(laststar.Center), origin, scale, SpriteEffects.None, 0);
				}
			}
			spriteBatch.Draw(Bloom, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(color * bloomopacity), 0, Bloom.Size() / 2, projectile.scale * bloomscale, SpriteEffects.None, 0);

			spriteBatch.Draw(MainTex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(Color.White * 0.5f), projectile.rotation * 1.5f, MainTex.Size() / 2, projectile.scale * 0.75f, SpriteEffects.None, 0);
			spriteBatch.Draw(MainTex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(Color.White * 0.5f), -projectile.rotation * 1.5f, MainTex.Size() / 2, projectile.scale * 0.75f, SpriteEffects.None, 0);

			spriteBatch.Draw(MainTex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(Color.White), projectile.rotation, MainTex.Size() / 2, projectile.scale, SpriteEffects.None, 0);

			if (Timer > 30 && Timer < 50)
			{
				float scale = 1 - Math.Abs(40 - Timer) / 10;
				spriteBatch.Draw(MainTex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(Color.White), -projectile.rotation * 2, MainTex.Size() / 2, projectile.scale * scale, SpriteEffects.None, 0);
			}
		}
	}
}