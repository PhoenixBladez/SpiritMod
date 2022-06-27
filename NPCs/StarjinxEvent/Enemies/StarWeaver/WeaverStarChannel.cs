using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Prim;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.StarWeaver
{
	public class WeaverStarChannel : ModProjectile, IDrawAdditive
	{
		public override string Texture => "Terraria/Projectile_1";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Star");

		private const int CHANNELTIME = StarWeaverNPC.STARBURST_CHANNELTIME;

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(10, 10);
			Projectile.hostile = true;
			Projectile.timeLeft = CHANNELTIME;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.scale = 0f;
			Projectile.hide = true;
		}

		private NPC Parent => Main.npc[(int)Projectile.ai[0]];

		private Player Target => Main.player[Parent.target];

		private int Timer => CHANNELTIME - Projectile.timeLeft;

		private ref float TargetAngle => ref Projectile.ai[1];

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => false;

		public override void AI()
		{
			if(Parent.type != ModContent.NPCType<StarWeaverNPC>() || !Parent.active || !Target.active)
			{
				Projectile.Kill();
				return;
			}

			if(Timer % 20 == 0 && !Main.dedServ)
				SoundEngine.PlaySound(SoundID.Item15.WithVolume(0.8f).WithPitchVariance(0.3f), Projectile.Center);

			Lighting.AddLight(Projectile.Center, Color.Goldenrod.ToVector3() / 3);
			Projectile.rotation += Parent.spriteDirection * 0.1f;
			Projectile.alpha = Math.Max(Projectile.alpha - (510 / CHANNELTIME), 0);
			Projectile.scale = Math.Min(Projectile.scale + (1 / (float)CHANNELTIME), 1);
			Projectile.Center = Parent.Center + new Vector2(9f * Parent.spriteDirection, 2 * (float)Math.Sin(Main.GameUpdateCount/7f));

			TargetAngle = Utils.AngleLerp(TargetAngle, Projectile.AngleTo(Target.Center), MathHelper.Lerp(0.2f, 0.04f, Timer / (float)CHANNELTIME));
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			StarPrimitive star = new StarPrimitive
			{
				Color = Color.White * Projectile.Opacity,
				TriangleHeight = 12 * Projectile.scale,
				TriangleWidth = 4 * Projectile.scale,
				Position = Projectile.Center - Main.screenPosition,
				Rotation = Projectile.rotation
			};
			PrimitiveRenderer.DrawPrimitiveShape(star);

			Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;

			sB.Draw(bloom, Projectile.Center - Main.screenPosition, null, Color.Goldenrod * 0.8f * Projectile.Opacity, 0, bloom.Size() / 2, 0.25f * Projectile.scale, SpriteEffects.None, 0);
			sB.Draw(bloom, Projectile.Center - Main.screenPosition, null, Color.Goldenrod * Projectile.Opacity, 0, bloom.Size() / 2, 0.2f * Projectile.scale, SpriteEffects.None, 0);

			Texture2D beam = Mod.Assets.Request<Texture2D>("Textures/Ray").Value;
			float beamProgress = (float)Math.Pow(Timer / (float)CHANNELTIME, 2);
			float opacity = MathHelper.Lerp(0.5f, 1f, beamProgress) * Projectile.Opacity;
			for (int i = 0; i < 5; i++)
			{
				float angle = TargetAngle + (MathHelper.TwoPi * (i / 5f)) - MathHelper.PiOver2;
				float length = MathHelper.Lerp(30, 150, beamProgress);
				Vector2 scale = new Vector2(Projectile.scale, length / beam.Height);
				sB.Draw(beam, Projectile.Center - Main.screenPosition, null, Color.Goldenrod * opacity, angle, new Vector2(beam.Width / 2, 0), scale, SpriteEffects.None, 0);
			}

			for (int i = 0; i < 5; i++)
			{
				float angle = TargetAngle + (MathHelper.TwoPi * ((i + 0.5f) / 5f)) - MathHelper.PiOver2;
				float length = MathHelper.Lerp(15, 100, beamProgress);
				Vector2 scale = new Vector2(Projectile.scale, length / beam.Height);
				sB.Draw(beam, Projectile.Center - Main.screenPosition, null, Color.Goldenrod * opacity, angle, new Vector2(beam.Width / 2, 0), scale, SpriteEffects.None, 0);
			}
		}

		public override void Kill(int timeLeft)
		{
			if (Parent.type != ModContent.NPCType<StarWeaverNPC>() || !Parent.active || !Target.active)
				return;

			if (!Main.dedServ)
			{
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithVolume(0.65f).WithPitchVariance(0.3f), Projectile.Center);

				for(int i = 0; i < 10; i++)
					ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Main.rand.NextVector2Circular(6, 6), Color.White, Color.Goldenrod, Main.rand.NextFloat(0.1f, 0.2f), 25));
			}

			for(int i = 0; i < 5; i++)
			{
				Vector2 vel = 26 * Vector2.UnitX.RotatedBy(TargetAngle + (MathHelper.TwoPi * (i / 5f)));
				Projectile.NewProjectileDirect(Projectile.Center, vel, ModContent.ProjectileType<WeaverStarFragment>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1f).netUpdate = true;
			}

			for (int i = 0; i < 5; i++)
			{
				Vector2 vel = Main.rand.NextFloat(8, 12) * Vector2.UnitX.RotatedBy(TargetAngle + (MathHelper.TwoPi * ((i + 0.5f) / 5f)));
				Projectile.NewProjectileDirect(Projectile.Center, vel, ModContent.ProjectileType<WeaverStarFragment>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack, Projectile.owner, 0.5f).netUpdate = true;
			}
		}
	}
}