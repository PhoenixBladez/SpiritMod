using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.BaseProj;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using SpiritMod.Particles;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SummonsMisc.FairyWhistle
{
	[AutoloadMinionBuff("Fairies", "Hey! Watch out!")]
	public class FairyMinion : BaseMinion, IDrawAdditive
	{
		public FairyMinion() : base(300, 500, new Vector2(20, 20)) { }

		public override void AbstractSetStaticDefaults()
		{
			DisplayName.SetDefault("Fairy");
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void AbstractSetDefaults() => Projectile.alpha = 255;

		public override bool DoAutoFrameUpdate(ref int framespersecond, ref int startframe, ref int endframe)
		{
			framespersecond = (int)MathHelper.Lerp(10, 20, Math.Min(Projectile.velocity.Length() / 6, 1));
			return true;
		}

		public override bool MinionContactDamage() => false;

		public override bool PreAI()
		{
			Projectile.rotation = Projectile.velocity.X * 0.05f;
			Projectile.alpha = Math.Max(Projectile.alpha - 3, 0);

			foreach (Projectile p in Main.projectile.Where(x => x.active && x != null && x.type == Projectile.type && x.owner == Projectile.owner && x != Projectile))
			{
				if (p.Hitbox.Intersects(Projectile.Hitbox))
					Projectile.velocity += Projectile.DirectionFrom(p.Center) / 10;
			}

			return true;
		}

		private ref float AiTimer => ref Projectile.ai[0];

		public override void IdleMovement(Player player)
		{
			AiTimer = 0;
			if(Math.Abs(Projectile.velocity.X) > 1) //dont flip too fast
				Projectile.direction = Projectile.spriteDirection = Math.Sign(Projectile.velocity.X) > 0 ? -1 : 1;

			Vector2 desiredPosition = player.MountedCenter - new Vector2(0, 60 + (float)Math.Sin(Main.GameUpdateCount / 6f) * 6);
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Lerp(Projectile.Center, desiredPosition, 0.15f) - Projectile.Center, 0.1f);
			if (Projectile.Distance(desiredPosition) > 600)
				Projectile.Center = desiredPosition;
		}

		private const int SHOOTTIME = 50;
		public override void TargettingBehavior(Player player, NPC target)
		{
			Projectile.direction = Projectile.spriteDirection = Math.Sign(Projectile.DirectionTo(target.Center).X) > 0 ? -1 : 1;
			Vector2 desiredPosition = player.MountedCenter - new Vector2(0, 60 + (float)Math.Sin(Main.GameUpdateCount / 6f) * 6);
			desiredPosition += Projectile.DirectionTo(target.Center) * 30;
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Lerp(Projectile.Center, desiredPosition, 0.15f) - Projectile.Center, 0.1f);
			if (++AiTimer >= SHOOTTIME)
			{
				Projectile.NewProjectile(Projectile.Center, Projectile.DirectionTo(target.Center) * 7, ModContent.ProjectileType<FairyProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				Projectile.velocity -= Projectile.DirectionTo(target.Center) * 4;
				AiTimer = 0;

				if (!Main.dedServ)
				{
					SoundEngine.PlaySound(SoundID.Item9.WithPitchVariance(0.3f), Projectile.Center);

					for (int i = 0; i < 10; i++)
						ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(1f, 2f),
							new Color(120, 239, 255) * 0.66f, new Color(94, 255, 126) * 0.66f, Main.rand.NextFloat(0.35f, 0.5f), 25, delegate (Particle p)
							{
								p.Velocity = p.Velocity.RotatedByRandom(0.1f) * 0.97f;
							}));
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDrawGlowTrail(Main.spriteBatch, 0.3f * Projectile.Opacity);
			Projectile.QuickDraw(Main.spriteBatch, drawColor: Color.White); 
			return false;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;
			float bloomOpacity = MathHelper.Clamp(1.5f * AiTimer / SHOOTTIME, 0.33f, 1f); //glow brighter when closer to shot time
			spriteBatch.Draw(bloom, Projectile.Center - Main.screenPosition, null, Color.Lerp(new Color(124, 255, 47) * Projectile.Opacity, Color.White, 0.75f) * bloomOpacity, 0, bloom.Size() / 2, 0.17f, SpriteEffects.None, 0);
		}
	}
}