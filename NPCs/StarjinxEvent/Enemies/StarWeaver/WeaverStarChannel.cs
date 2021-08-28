using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.StarWeaver
{
	public class WeaverStarChannel : ModProjectile, IBasicPrimDraw, IDrawAdditive
	{
		public override string Texture => "Terraria/Projectile_1";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Star");

		private const int CHANNELTIME = StarWeaverNPC.STARBURST_CHANNELTIME;

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(10, 10);
			projectile.hostile = true;
			projectile.timeLeft = CHANNELTIME;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.scale = 0f;
			projectile.hide = true;
		}

		private NPC Parent => Main.npc[(int)projectile.ai[0]];

		private Player Target => Main.player[Parent.target];

		private int Timer => CHANNELTIME - projectile.timeLeft;

		private ref float TargetAngle => ref projectile.ai[1];

		public override bool CanDamage() => false;

		public override void AI()
		{
			if(Parent.type != ModContent.NPCType<StarWeaverNPC>() || !Parent.active || !Target.active)
			{
				projectile.Kill();
				return;
			}

			if(Timer % 20 == 0 && !Main.dedServ)
				Main.PlaySound(SoundID.Item15.WithVolume(0.8f).WithPitchVariance(0.3f), projectile.Center);

			Lighting.AddLight(projectile.Center, Color.Goldenrod.ToVector3() / 3);
			projectile.rotation += Parent.spriteDirection * 0.1f;
			projectile.alpha = Math.Max(projectile.alpha - (510 / CHANNELTIME), 0);
			projectile.scale = Math.Min(projectile.scale + (1 / (float)CHANNELTIME), 1);
			projectile.Center = Parent.Center + new Vector2(9f * Parent.spriteDirection, 2 * (float)Math.Sin(Main.GameUpdateCount/7f));

			TargetAngle = Utils.AngleLerp(TargetAngle, projectile.AngleTo(Target.Center), MathHelper.Lerp(0.2f, 0.04f, Timer / (float)CHANNELTIME));
		}

		public void DrawPrimShape(BasicEffect effect) => StarDraw.DrawStarBasic(effect, projectile.Center, projectile.rotation, projectile.scale * 15, Color.White * projectile.Opacity);

		public void AdditiveCall(SpriteBatch sB)
		{
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");

			sB.Draw(bloom, projectile.Center - Main.screenPosition, null, Color.Goldenrod * 0.8f * projectile.Opacity, 0, bloom.Size() / 2, 0.25f * projectile.scale, SpriteEffects.None, 0);
			sB.Draw(bloom, projectile.Center - Main.screenPosition, null, Color.Goldenrod * projectile.Opacity, 0, bloom.Size() / 2, 0.2f * projectile.scale, SpriteEffects.None, 0);

			Texture2D beam = mod.GetTexture("Textures/Ray");
			float beamProgress = (float)Math.Pow(Timer / (float)CHANNELTIME, 2);
			float opacity = MathHelper.Lerp(0.5f, 1f, beamProgress) * projectile.Opacity;
			for (int i = 0; i < 5; i++)
			{
				float angle = TargetAngle + (MathHelper.TwoPi * (i / 5f)) - MathHelper.PiOver2;
				float length = MathHelper.Lerp(30, 150, beamProgress);
				Vector2 scale = new Vector2(projectile.scale, length / beam.Height);
				sB.Draw(beam, projectile.Center - Main.screenPosition, null, Color.Goldenrod * opacity, angle, new Vector2(beam.Width / 2, 0), scale, SpriteEffects.None, 0);
			}

			for (int i = 0; i < 5; i++)
			{
				float angle = TargetAngle + (MathHelper.TwoPi * ((i + 0.5f) / 5f)) - MathHelper.PiOver2;
				float length = MathHelper.Lerp(15, 100, beamProgress);
				Vector2 scale = new Vector2(projectile.scale, length / beam.Height);
				sB.Draw(beam, projectile.Center - Main.screenPosition, null, Color.Goldenrod * opacity, angle, new Vector2(beam.Width / 2, 0), scale, SpriteEffects.None, 0);
			}
		}

		public override void Kill(int timeLeft)
		{
			if (Parent.type != ModContent.NPCType<StarWeaverNPC>() || !Parent.active || !Target.active)
				return;

			if (!Main.dedServ)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithVolume(0.65f).WithPitchVariance(0.3f), projectile.Center);

				for(int i = 0; i < 10; i++)
					ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, Main.rand.NextVector2Circular(6, 6), Color.White, Color.Goldenrod, Main.rand.NextFloat(0.1f, 0.2f), 25));
			}

			for(int i = 0; i < 5; i++)
			{
				Vector2 vel = 26 * Vector2.UnitX.RotatedBy(TargetAngle + (MathHelper.TwoPi * (i / 5f)));
				Projectile.NewProjectileDirect(projectile.Center, vel, ModContent.ProjectileType<WeaverStarFragment>(), projectile.damage, projectile.knockBack, projectile.owner, 1f).netUpdate = true;
			}

			for (int i = 0; i < 5; i++)
			{
				Vector2 vel = Main.rand.NextFloat(8, 12) * Vector2.UnitX.RotatedBy(TargetAngle + (MathHelper.TwoPi * ((i + 0.5f) / 5f)));
				Projectile.NewProjectileDirect(projectile.Center, vel, ModContent.ProjectileType<WeaverStarFragment>(), (int)(projectile.damage * 0.75f), projectile.knockBack, projectile.owner, 0.5f).netUpdate = true;
			}
		}
	}
}