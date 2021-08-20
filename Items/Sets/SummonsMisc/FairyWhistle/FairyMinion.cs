using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.BaseProj;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using SpiritMod.Particles;

namespace SpiritMod.Items.Sets.SummonsMisc.FairyWhistle
{
	[AutoloadMinionBuff("Fairies", "Hey! Watch out!")]
	public class FairyMinion : BaseMinion, IDrawAdditive
	{
		public FairyMinion() : base(300, 500, new Vector2(15, 15)) { }

		public override void AbstractSetStaticDefaults()
		{
			DisplayName.SetDefault("Fairy");
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void AbstractSetDefaults() => projectile.alpha = 255;

		public override bool DoAutoFrameUpdate(ref int framespersecond, ref int startframe, ref int endframe)
		{
			framespersecond = (int)MathHelper.Lerp(6, 14, Math.Min(projectile.velocity.Length() / 6, 1));
			return true;
		}

		public override bool MinionContactDamage() => false;

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.X * 0.05f;
			projectile.alpha = Math.Max(projectile.alpha - 3, 0);
			if (Main.GameUpdateCount % 40 == 0 && !Main.dedServ)
				ParticleHandler.SpawnParticle(new PulseCircle(projectile, Color.Lerp(new Color(124, 255, 47), Color.White, 0.66f) * 0.6f * projectile.Opacity, 45, 120, PulseCircle.MovementType.Inwards));

			foreach (Projectile p in Main.projectile.Where(x => x.active && x != null && x.type == projectile.type && x.owner == projectile.owner && x != projectile))
			{
				if (p.Hitbox.Intersects(projectile.Hitbox))
					projectile.velocity += projectile.DirectionFrom(p.Center) / 5;
			}

			return true;
		}

		private ref float AiTimer => ref projectile.ai[0];

		public override void IdleMovement(Player player)
		{
			AiTimer = 0;
			if(Math.Abs(projectile.velocity.X) > 1) //dont flip too fast
				projectile.direction = projectile.spriteDirection = Math.Sign(projectile.velocity.X) > 0 ? -1 : 1;

			Vector2 desiredPosition = player.MountedCenter - new Vector2(0, 60 + (float)Math.Sin(Main.GameUpdateCount / 6f) * 6);
			projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Lerp(projectile.Center, desiredPosition, 0.15f) - projectile.Center, 0.1f);
			if (projectile.Distance(desiredPosition) > 600)
				projectile.Center = desiredPosition;
		}

		private const int SHOOTTIME = 50;
		public override void TargettingBehavior(Player player, NPC target)
		{
			projectile.direction = projectile.spriteDirection = Math.Sign(projectile.DirectionTo(target.Center).X) > 0 ? -1 : 1;
			Vector2 desiredPosition = player.MountedCenter - new Vector2(0, 60 + (float)Math.Sin(Main.GameUpdateCount / 6f) * 6);
			desiredPosition += projectile.DirectionTo(target.Center) * 30;
			projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Lerp(projectile.Center, desiredPosition, 0.15f) - projectile.Center, 0.1f);
			if (++AiTimer >= SHOOTTIME)
			{
				Projectile.NewProjectile(projectile.Center, projectile.DirectionTo(target.Center) * 12, 1, projectile.damage, projectile.knockBack, projectile.owner);
				projectile.velocity -= projectile.DirectionTo(target.Center) * 4;
				AiTimer = 0;

				if(!Main.dedServ)
					for(int i = 0; i < 10; i++)
						ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(1f, 2f), 
							new Color(120, 239, 255), new Color(94, 255, 126), Main.rand.NextFloat(0.5f, 0.6f), 25, delegate (Particle p)
							{
								p.Velocity = p.Velocity.RotatedByRandom(0.1f) * 0.97f;
							}));
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDrawGlowTrail(spriteBatch, 0.3f * projectile.Opacity);
			projectile.QuickDraw(spriteBatch, drawColor: Color.White); 
			return false;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			float bloomOpacity = MathHelper.Clamp(1.5f * AiTimer / SHOOTTIME, 0.33f, 1f); //glow brighter when closer to shot time
			spriteBatch.Draw(bloom, projectile.Center - Main.screenPosition, null, Color.Lerp(new Color(124, 255, 47) * projectile.Opacity, Color.White, 0.75f) * bloomOpacity, 0, bloom.Size() / 2, 0.2f, SpriteEffects.None, 0);
		}
	}
}