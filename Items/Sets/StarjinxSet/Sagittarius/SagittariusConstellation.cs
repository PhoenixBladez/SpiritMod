using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
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
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.timeLeft = 600;
			Projectile.scale = Main.rand.NextFloat(0.18f, 0.28f);
			Projectile.alpha = 255;
			Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			Projectile.hide = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => false;

		private Vector2 _position;

		private Player Owner => Main.player[Projectile.owner];

		private ref float StarsLeft => ref Projectile.ai[0];

		private ref float LastStar => ref Projectile.ai[1];

		private ref float Timer => ref Projectile.localAI[1];

		private Color BloomColor => (StarsLeft % 2 == 1) ? new Color(0, 242, 255) : new Color(255, 92, 211);

		public override bool PreAI()
		{
			if (Projectile.localAI[0] == 0)
			{
				_position = Projectile.Center - Owner.MountedCenter;
				Projectile.localAI[0]++;
				Projectile.netUpdate = true;
			}

			Projectile.Center = Owner.MountedCenter + _position;
			return true;
		}

		public override void AI()
		{
			Projectile.rotation += 0.08f * Owner.direction * ((StarsLeft % 2 == 0) ? 1 : -1);
			if (++Timer < 20)
			{
				Projectile.scale *= 1.05f;
				Projectile.alpha = Math.Max(Projectile.alpha - 10, 0);
			}

			if (Timer == 30 && !Main.dedServ) //particle effects on arrow shot time
			{
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/MagicCast1").WithPitchVariance(0.2f), Projectile.Center);
				Color color = Color.Lerp(BloomColor, Color.White, 0.5f);
				for (int i = 0; i < 7; i++)
					ParticleHandler.SpawnParticle(new ImpactLine(Projectile.Center, Main.rand.NextVector2Unit(), color * 0.7f, new Vector2(0.2f, Main.rand.NextFloat(0.3f, 0.4f)), 10, Projectile));

				ParticleHandler.SpawnParticle(new PulseCircle(Projectile, color * 0.5f, Projectile.scale * 80, 15, PulseCircle.MovementType.OutwardsQuadratic));
			}

			if (Owner == Main.LocalPlayer)
			{
				if (Timer == 10 && StarsLeft > 1) //spawn next constellation star
				{
					Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Owner.MountedCenter - Owner.DirectionTo(Main.MouseWorld).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(70, 100),
						Vector2.Zero, ModContent.ProjectileType<SagittariusConstellation>(), Projectile.damage, Projectile.knockBack, Projectile.owner, StarsLeft - 1, Projectile.whoAmI).netUpdate = true;
					Projectile.netUpdate = true;
				}

				if (Timer == 30) //fire the arrow
				{
					float baseAngle = Owner.AngleTo(Main.MouseWorld);
					float starAngle = Projectile.AngleTo(Main.MouseWorld);
					float angletoshoot = (MathHelper.WrapAngle(baseAngle - starAngle) > 0) ? MathHelper.PiOver2 : -MathHelper.PiOver2;
					Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld).RotatedBy(angletoshoot) * 16,
						ModContent.ProjectileType<SagittariusConstellationArrow>(), Projectile.damage, Projectile.knockBack, Projectile.owner, StarsLeft % 2);

					if (proj.ModProjectile is SagittariusConstellationArrow)
						(proj.ModProjectile as SagittariusConstellationArrow).TargetPos = Main.MouseWorld;

					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);

					Projectile.netUpdate = true;
				}
			}

			if (Timer > 50)
			{
				Projectile.scale *= 0.98f;
				Projectile.alpha += 10;
				if (Projectile.alpha >= 255)
					Projectile.Kill();
			}
		}

		float ScaleProgress() => (float)Math.Pow(1 - Math.Abs(30 - Timer) / 15, 3);
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D Bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;
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
				if (laststar.active && laststar.type == Projectile.type && laststar.ai[0] == Projectile.ai[0] + 1 && laststar.owner == Projectile.owner)
				{
					Texture2D Beam = Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_4").Value;
					Vector2 scale = new Vector2(Projectile.Distance(laststar.Center) / Beam.Width, Projectile.scale * 30 / Beam.Height);
					float opacity = 0.8f * Projectile.Opacity * laststar.Opacity;
					Vector2 origin = new Vector2(0, Beam.Height / 2);
					spriteBatch.Draw(Beam, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.White, color, 0.75f) * opacity, Projectile.AngleTo(laststar.Center), origin, scale, SpriteEffects.None, 0);
				}
			}

			int bloomstodraw = 3;
			for(int i = 0; i < bloomstodraw; i++)
			{
				float progress = i / (float)bloomstodraw;
				spriteBatch.Draw(Bloom, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color * bloomopacity) * MathHelper.Lerp(1f, 0.5f, 1 - progress), 0,
					Bloom.Size() / 2, Projectile.scale * bloomscale * MathHelper.Lerp(0.75f, 1.2f, progress), SpriteEffects.None, 0);
			}


			StarPrimitive star = new StarPrimitive
			{
				Color = Color.White * Projectile.Opacity,
				TriangleHeight = starScale * Projectile.scale,
				TriangleWidth = starScale * Projectile.scale * 0.3f,
				Position = Projectile.Center - Main.screenPosition,
				Rotation = Projectile.rotation
			};
			PrimitiveRenderer.DrawPrimitiveShape(star);
		}
	}
}