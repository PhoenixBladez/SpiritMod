using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.StarjinxSet.Sagittarius
{
	public class SagittariusConstellation : ModProjectile, IDrawAdditive
	{
		public override string Texture => "Terraria/Projectile_1";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Constellation");

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.timeLeft = 600;
			projectile.scale = Main.rand.NextFloat(0.18f, 0.28f);
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

		private Color BloomColor => (StarsLeft % 2 == 1) ? new Color(0, 242, 255) : new Color(255, 92, 211);

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
			projectile.rotation += 0.08f * Owner.direction * ((StarsLeft % 2 == 0) ? 1 : -1);
			if (++Timer < 20)
			{
				projectile.scale *= 1.05f;
				projectile.alpha = Math.Max(projectile.alpha - 10, 0);
			}

			if (Timer == 30 && !Main.dedServ) //particle effects on arrow shot time
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starCast").WithPitchVariance(0.2f).WithVolume(0.8f), projectile.Center);
				Color color = Color.Lerp(BloomColor, Color.White, 0.5f);
				for (int i = 0; i < 7; i++)
					ParticleHandler.SpawnParticle(new ImpactLine(projectile.Center, Main.rand.NextVector2Unit(), color * 0.7f, new Vector2(0.2f, Main.rand.NextFloat(0.3f, 0.4f)), 10, projectile));

				ParticleHandler.SpawnParticle(new PulseCircle(projectile, color * 0.5f, projectile.scale * 80, 15, PulseCircle.MovementType.OutwardsQuadratic));
			}

			if (Owner == Main.LocalPlayer)
			{
				if (Timer == 10 && StarsLeft > 1) //spawn next constellation star
				{
					Projectile.NewProjectileDirect(Owner.MountedCenter - Owner.DirectionTo(Main.MouseWorld).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(70, 100),
						Vector2.Zero, ModContent.ProjectileType<SagittariusConstellation>(), projectile.damage, projectile.knockBack, projectile.owner, StarsLeft - 1, projectile.whoAmI).netUpdate = true;
					projectile.netUpdate = true;
				}

				if (Timer == 30) //fire the arrow
				{
					float baseAngle = Owner.AngleTo(Main.MouseWorld);
					float starAngle = projectile.AngleTo(Main.MouseWorld);
					float angletoshoot = (MathHelper.WrapAngle(baseAngle - starAngle) > 0) ? MathHelper.PiOver2 : -MathHelper.PiOver2;
					Projectile proj = Projectile.NewProjectileDirect(projectile.Center, projectile.DirectionTo(Main.MouseWorld).RotatedBy(angletoshoot) * 16,
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

		float ScaleProgress() => (float)Math.Pow(1 - Math.Abs(30 - Timer) / 15, 3);
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D Bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			Color color = BloomColor;
			float bloomopacity = 0.75f;
			float bloomscale = 0.4f;
			float starScale = 15f;
			if (Timer > 15 && Timer < 45)
			{
				color = Color.Lerp(color, Color.White, 0.75f * ScaleProgress());
				bloomopacity = MathHelper.Lerp(bloomopacity, 2f, ScaleProgress());
				bloomscale = MathHelper.Lerp(bloomscale, 0.25f, ScaleProgress());
				starScale = MathHelper.Lerp(starScale, 7f, ScaleProgress());
			}

			if (LastStar >= 0)
			{
				Projectile laststar = Main.projectile[(int)LastStar];
				if (laststar.active && laststar.type == projectile.type && laststar.ai[0] == projectile.ai[0] + 1 && laststar.owner == projectile.owner)
				{
					Texture2D Beam = mod.GetTexture("Textures/Trails/Trail_4");
					Vector2 scale = new Vector2(projectile.Distance(laststar.Center) / Beam.Width, projectile.scale * 30 / Beam.Height);
					float opacity = 0.8f * projectile.Opacity * laststar.Opacity;
					Vector2 origin = new Vector2(0, Beam.Height / 2);
					spriteBatch.Draw(Beam, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.White, color, 0.75f) * opacity, projectile.AngleTo(laststar.Center), origin, scale, SpriteEffects.None, 0);
				}
			}

			int bloomstodraw = 3;
			for(int i = 0; i < bloomstodraw; i++)
			{
				float progress = i / (float)bloomstodraw;
				spriteBatch.Draw(Bloom, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(color * bloomopacity) * MathHelper.Lerp(1f, 0.5f, 1 - progress), 0,
					Bloom.Size() / 2, projectile.scale * bloomscale * MathHelper.Lerp(0.75f, 1.2f, progress), SpriteEffects.None, 0);
			}


			StarPrimitive star = new StarPrimitive
			{
				Color = Color.White * projectile.Opacity,
				TriangleHeight = starScale * projectile.scale,
				TriangleWidth = starScale * projectile.scale * 0.3f,
				Position = projectile.Center - Main.screenPosition,
				Rotation = projectile.rotation
			};
			PrimitiveRenderer.DrawPrimitiveShape(star);
		}
	}
}