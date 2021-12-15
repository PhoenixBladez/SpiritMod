using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.BaseProj;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SummonsMisc.SoulDagger
{
	[AutoloadMinionBuff("Soul Dagger", "Reap their souls!")]
	public class SoulDaggerProj : BaseMinion
	{
		public enum Phases
		{
			IDLE = 0,
			TRANSITIONING = 1,
			ATTACKING = 2
		}


		public SoulDaggerProj() : base(1000, 1500, new Vector2(64, 64)) { }

		private Phases phase;

		int frameY = 0;

		private int NumFrames //10 frames on summon, future self
		{
			get
			{
				switch (phase)
				{
					case Phases.IDLE:
						return 21;
					case Phases.TRANSITIONING:
						return 9;
					case Phases.ATTACKING:
						return 9;
					default:
						return 1;
				}
			}
		}

		private string TextureEnd
		{
			get
			{
				switch (phase)
				{
					case Phases.IDLE:
						return "_Idle";
					case Phases.TRANSITIONING:
						return "_Transition";
					case Phases.ATTACKING:
						return "_Attack";
					default:
						return "";
				}
			}
		}

		public override void AbstractSetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Dagger");
			Main.projFrames[projectile.type] = 1;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void AbstractSetDefaults() => projectile.localNPCHitCooldown = 20;

		public override bool MinionContactDamage() => true;

		public override void IdleMovement(Player player)
		{
			switch (phase)
			{
				case Phases.ATTACKING:
					AttackBehaviorToIdle(player);
					return;
				case Phases.TRANSITIONING:
					phase = Phases.IDLE;
					break;
			}
			projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(player.Center) * 10, 0.03f);
			projectile.rotation = projectile.velocity.X / 20f;
			projectile.frameCounter++;
			if (projectile.frameCounter % 3 == 2)
			{
				frameY++;
			}
			frameY %= NumFrames;
		}

		public override void TargettingBehavior(Player player, NPC target)
		{
			switch (phase)
			{
				case Phases.IDLE:
					phase = Phases.TRANSITIONING;
					frameY = 0;
					projectile.frameCounter = 0;
					break;
				case Phases.TRANSITIONING:
					TransitionBehavior(player, target);
					break;
				case Phases.ATTACKING:
					AttackBehavior(player, target);
					break;
			}
		}

		private void TransitionBehavior(Player player, NPC target)
		{
			projectile.velocity = Vector2.Zero;
			projectile.frameCounter++;
			if (projectile.frameCounter % 3 == 2)
			{
				if (frameY >= NumFrames)
				{
					phase = Phases.ATTACKING;
					frameY = 0;
					projectile.frameCounter = 0;
					projectile.Center = target.Center;
					projectile.rotation = Main.rand.NextFloat(6.28f);
				}
				else
					frameY++;
			}
		}

		private void AttackBehavior(Player player, NPC target)
		{
			projectile.velocity = Vector2.Zero;
			projectile.frameCounter++;
			if (projectile.frameCounter % 3 == 2)
			{
				if (frameY >= NumFrames)
				{
					projectile.Center = target.Center;
					projectile.rotation = Main.rand.NextFloat(6.28f);
					frameY = 0;
					projectile.frameCounter = 0;
				}
				else
					frameY++;
			}
		}

		private void AttackBehaviorToIdle(Player player)
		{
			projectile.velocity = Vector2.Zero;
			projectile.frameCounter++;
			if (projectile.frameCounter % 3 == 2)
			{
				if (frameY >= NumFrames)
				{
					phase = Phases.IDLE;
					frameY = 0;
					projectile.frameCounter = 0;
				}
				else
					frameY++;
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (phase == Phases.ATTACKING && frameY == 4)
				return base.Colliding(projHitbox, targetHitbox);
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = ModContent.GetTexture(Texture + TextureEnd);
			int frameHeight = tex.Height / NumFrames;
			Rectangle frame = new Rectangle(0, frameHeight * frameY, tex.Width, frameHeight);

			Vector2 origin = new Vector2(tex.Width, frameHeight) / 2;
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, frame, Color.White, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}