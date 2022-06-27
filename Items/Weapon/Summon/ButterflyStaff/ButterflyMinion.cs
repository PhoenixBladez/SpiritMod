using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.BaseProj;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Weapon.Summon.ButterflyStaff
{
	[AutoloadMinionBuff("Etherial Butterfly", "A glowing butterfly fights for you!")]
	public class ButterflyMinion : BaseMinion, IDrawAdditive
	{
		public ButterflyMinion() : base(600, 800, new Vector2(16, 16)) { }
		public override void AbstractSetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Butterfly");
			Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override bool PreAI()
		{
			if (AiState == Moving)
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
				Projectile.alpha = Math.Max(Projectile.alpha - 5, 0);

				foreach (Projectile p in Main.projectile.Where(x => x.active && x != null && x.type == Projectile.type && x.owner == Projectile.owner && x != Projectile))
					if (p.Hitbox.Intersects(Projectile.Hitbox))
						Projectile.velocity += Projectile.DirectionFrom(p.Center) / 20;

				if (Main.rand.NextBool(8) && !Main.dedServ)
					Particles.ParticleHandler.SpawnParticle(new Particles.StarParticle(Projectile.Center + Main.rand.NextVector2Circular(4, 4),
						Projectile.velocity.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(0.2f, 0.4f), Color.LightPink, Color.DeepPink, Main.rand.NextFloat(0.1f, 0.2f), 20));
			}
			else
				Projectile.alpha = Math.Min(Projectile.alpha + 5, 100);

			return true;
		}

		private ref float AiState => ref Projectile.ai[0];
		private const float Moving = 0;
		private const float StuckToPlayer = 1;

		private Vector2 stuckPos = Vector2.Zero;
		public override void IdleMovement(Player player)
		{
			if (AiState == Moving)
			{
				if (Projectile.Distance(player.MountedCenter) > 2000)
					Projectile.Center = player.MountedCenter;

				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.MountedCenter) * 15, 0.04f);
				if (Projectile.Hitbox.Intersects(player.Hitbox))
				{
					stuckPos = Projectile.Center - player.MountedCenter;
					AiState = StuckToPlayer;
					Projectile.netUpdate = true;
				}
			}
			else
				Projectile.Center = stuckPos + player.MountedCenter;
		}

		public override bool DoAutoFrameUpdate(ref int framespersecond, ref int startframe, ref int endframe)
		{
			framespersecond = 6;
			return true;
		}

		public override void TargettingBehavior(Player player, NPC target)
		{
			AiState = Moving;

			if (Math.Abs(MathHelper.WrapAngle(Projectile.velocity.ToRotation() - Projectile.AngleTo(target.Center))) < MathHelper.PiOver4) //if close enough in desired angle, accelerate and home accurately
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center) * 18, 0.1f);

			else //if too much of an angle, circle around
			{
				if (Projectile.velocity.Length() > 8)
					Projectile.velocity *= 0.97f;

				if (Projectile.velocity.Length() < 5)
					Projectile.velocity *= 1.04f;

				Projectile.velocity = Projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center) * Projectile.velocity.Length(), 0.125f));
			}
		}

		public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.Opacity;

		public override void Kill(int timeLeft)
		{
			DustHelper.DrawStar(Projectile.Center, 223, pointAmount: 5, mainSize: 1.6425f, dustDensity: 1.5f, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);
			for (int i = 0; i < 15; i++)
				Dust.NewDustPerfect(Projectile.Center, 223, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, 0.5f);
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			Texture2D bloom = Mod.GetTexture("Effects/Masks/CircleGradient");
			sB.Draw(bloom, Projectile.Center - Main.screenPosition, null, Color.LightPink * Projectile.Opacity * 0.6f, 0, bloom.Size() / 2, 0.2f, SpriteEffects.None, 0);

			Projectile.QuickDrawTrail(sB, AiState == Moving ? 0.6f : 0f);
			Projectile.QuickDraw(sB);
		}
	}
}