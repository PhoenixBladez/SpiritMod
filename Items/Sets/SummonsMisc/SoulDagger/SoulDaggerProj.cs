using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.BaseProj;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
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
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void AbstractSetDefaults() => Projectile.localNPCHitCooldown = 20;

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
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.Center - new Vector2(player.direction * 30, 0)) * 10, 0.03f);
			Projectile.rotation = Projectile.velocity.X / 20f;
			Projectile.frameCounter++;
			if (Projectile.frameCounter % 3 == 2)
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
					Projectile.frameCounter = 0;
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
			Projectile.velocity = Vector2.Zero;
			Projectile.frameCounter++;
			if (Projectile.frameCounter % 3 == 2)
			{
				if (frameY >= NumFrames)
				{
					phase = Phases.ATTACKING;
					frameY = 0;
					Projectile.frameCounter = 0;
					Projectile.Center = target.Center;
					Projectile.rotation = Main.rand.NextFloat(6.28f);
					Projectile.NewProjectile(Projectile.Center - (Projectile.rotation.ToRotationVector2() * 150), (Projectile.rotation + 1.57f).ToRotationVector2(), ModContent.ProjectileType<SoulDaggerSummon>(), 0, 0, player.whoAmI);
				}
				else
					frameY++;
			}
		}

		private void AttackBehavior(Player player, NPC target)
		{
			Projectile.velocity = Vector2.Zero;
			Projectile.frameCounter++;
			if (Projectile.frameCounter % 3 == 2)
			{
				if (frameY >= NumFrames)
				{
					Projectile.Center = target.Center;
					Projectile.rotation = Main.rand.NextFloat(6.28f);
					Projectile.NewProjectile(Projectile.Center - (Projectile.rotation.ToRotationVector2() * 150), (Projectile.rotation + 1.57f).ToRotationVector2(), ModContent.ProjectileType<SoulDaggerSummon>(), 0, 0, player.whoAmI);
					frameY = 0;
					Projectile.frameCounter = 0;
				}
				else
					frameY++;
			}
		}

		private void AttackBehaviorToIdle(Player player)
		{
			Projectile.velocity = Vector2.Zero;
			Projectile.frameCounter++;
			if (Projectile.frameCounter % 3 == 2)
			{
				if (frameY >= NumFrames)
				{
					phase = Phases.IDLE;
					frameY = 0;
					Projectile.frameCounter = 0;
					Projectile.Center = player.Center + Main.rand.NextVector2Circular(80, 80);
					Projectile.NewProjectile(Projectile.Center, new Vector2(0, 1), ModContent.ProjectileType<SoulDaggerSummon>(), 0, 0, player.whoAmI);
				}
				else
					frameY++;
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float collisionPoint = 0f;
			if (phase == Phases.ATTACKING && frameY == 4)
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + (Projectile.rotation.ToRotationVector2() * 150), Projectile.Center - (Projectile.rotation.ToRotationVector2() * 150), 30, ref collisionPoint);
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>(Texture + TextureEnd);
			int frameHeight = tex.Height / NumFrames;
			Rectangle frame = new Rectangle(0, frameHeight * frameY, tex.Width, frameHeight);

			Vector2 origin = new Vector2(tex.Width, frameHeight) / 2;
			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
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
			Main.projFrames[Projectile.type] = 10;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = false;
			Projectile.Size = new Vector2(100, 100);
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 16;
		}
		public override void AI()
		{
			if (Projectile.velocity != Vector2.Zero)
				Projectile.rotation = Projectile.velocity.ToRotation() - 1.57f;
			Projectile.velocity = Vector2.Zero;
			Projectile.frameCounter++;
			if (Projectile.frameCounter % 3 == 0)
				Projectile.frame++;
			if (Projectile.frame >= Main.projFrames[Projectile.type])
				Projectile.active = false;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, tex.Width, frameHeight);
			spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, frame, color, Projectile.rotation, new Vector2(0, frameHeight), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}