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
			projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(player.Center - new Vector2(player.direction * 30, 0)) * 10, 0.03f);
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
					Projectile.NewProjectile(projectile.Center - (projectile.rotation.ToRotationVector2() * 150), (projectile.rotation + 1.57f).ToRotationVector2(), ModContent.ProjectileType<SoulDaggerSummon>(), 0, 0, player.whoAmI);
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
					Projectile.NewProjectile(projectile.Center - (projectile.rotation.ToRotationVector2() * 150), (projectile.rotation + 1.57f).ToRotationVector2(), ModContent.ProjectileType<SoulDaggerSummon>(), 0, 0, player.whoAmI);
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
					projectile.Center = player.Center + Main.rand.NextVector2Circular(80, 80);
					Projectile.NewProjectile(projectile.Center, new Vector2(0, 1), ModContent.ProjectileType<SoulDaggerSummon>(), 0, 0, player.whoAmI);
				}
				else
					frameY++;
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float collisionPoint = 0f;
			if (phase == Phases.ATTACKING && frameY == 4)
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center + (projectile.rotation.ToRotationVector2() * 150), projectile.Center - (projectile.rotation.ToRotationVector2() * 150), 30, ref collisionPoint);
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
	internal class SoulDaggerSummon : ModProjectile
	{

		protected virtual Color color => Color.White;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Dagger");
			Main.projFrames[projectile.type] = 10;
		}

		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.ranged = true;
			projectile.tileCollide = false;
			projectile.Size = new Vector2(100, 100);
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 16;
		}
		public override void AI()
		{
			if (projectile.velocity != Vector2.Zero)
				projectile.rotation = projectile.velocity.ToRotation() - 1.57f;
			projectile.velocity = Vector2.Zero;
			projectile.frameCounter++;
			if (projectile.frameCounter % 3 == 0)
				projectile.frame++;
			if (projectile.frame >= Main.projFrames[projectile.type])
				projectile.active = false;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, frame, color, projectile.rotation, new Vector2(0, frameHeight), projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}