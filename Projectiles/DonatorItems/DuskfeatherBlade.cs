using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using static SpiritMod.Projectiles.DonatorItems.DuskfeatherBlade.DuskfeatherState;

namespace SpiritMod.Projectiles.DonatorItems
{
	class DuskfeatherBlade : ModProjectile
	{
		public static int _type;

		private const float Range = 25 * 16;
		private const float Max_Dist = 100 * 16;
		private const int Total_Updates = 3;
		private const int Total_Lifetime = 3600 * Total_Updates;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duskfeather Blade");
			Main.projFrames[projectile.type] = 13;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.height = 14;
			projectile.width = 14;
			projectile.alpha = 255;
			projectile.penetrate = 2;
			projectile.aiStyle = -1;
			projectile.extraUpdates = Total_Updates - 1;
			projectile.timeLeft = Total_Lifetime;
		}

		internal static void AttractBlades(Player player)
		{
			for (int i = 0; i < Main.maxProjectiles; ++i)
			{
				var projectile = Main.projectile[i];
				if (!projectile.active)
					continue;
				int state = (int)projectile.ai[0];
				if (projectile.type == _type &&
					projectile.owner == player.whoAmI &&
					state != (int)FadeOut &&
					state != (int)FadeOutStuck)
				{
					Retract(projectile);
				}
			}
		}

		internal static void AttractOldestBlade(Player player)
		{
			Projectile oldest = null;
			int timeLeft = int.MaxValue;
			for (int i = 0; i < Main.maxProjectiles; ++i)
			{
				var projectile = Main.projectile[i];
				if (!projectile.active)
					continue;
				int state = (int)projectile.ai[0];
				if (projectile.type == _type &&
					projectile.owner == player.whoAmI &&
					state != (int)Return &&
					state != (int)FadeOut &&
					state != (int)FadeOutStuck &&
					projectile.timeLeft < timeLeft)
				{
					timeLeft = projectile.timeLeft;
					oldest = projectile;
				}
			}
			if (oldest != null)
				Retract(oldest);
		}

		internal static void Retract(Projectile projectile)
		{
			projectile.ai[0] = (int)Return;
			projectile.netUpdate = true;
		}


		public override void Kill(int timeLeft)
		{
			if (projectile.alpha == 255)
				return;
		}

		private DuskfeatherState State
		{
			get { return (DuskfeatherState)(int)projectile.ai[0]; }
			set { projectile.ai[0] = (int)value; }
		}
		private float FiringVelocity
		{
			get { return projectile.ai[1]; }
			set { projectile.ai[1] = value; }
		}
		private Vector2 Origin
		{
			get { return new Vector2(projectile.localAI[0], projectile.localAI[1]); }
			set
			{
				projectile.localAI[0] = value.X;
				projectile.localAI[1] = value.Y;
			}
		}
		private float Poof
		{
			get { return projectile.localAI[0]; }
			set { projectile.localAI[0] = value; }
		}
		public override void AI()
		{
			if (State < Return)
			{
				if (projectile.alpha > 25)
					projectile.alpha -= 25;
				else
					projectile.alpha = 0;
			}
			int minFrame = 7;
			int maxFrame = 12;
			switch (State)
			{
				case Moving:
					AIMove();
					break;
				case StuckInBlock:
					maxFrame = 7;
					AIStopped();
					break;
				case Stopped:
					minFrame = 0;
					maxFrame = 6;
					AIStopped();
					break;
				case Return:
					AIReturn();
					break;
				case FadeOut:
					minFrame = 0;
					maxFrame = 6;
					AIFade();
					break;
				case FadeOutStuck:
					maxFrame = 7;
					AIFade();
					break;
			}
			if (projectile.numUpdates == 0)
			{
				if (State == Moving || State == Return)
					++projectile.frameCounter;
				if (++projectile.frameCounter >= 5)
				{
					projectile.frameCounter = 0;
					++projectile.frame;
				}
				if (projectile.frame < minFrame || projectile.frame > maxFrame)
					projectile.frame = minFrame;
			}
		}

		private void AIMove()
		{
			if (Origin == Vector2.Zero)
			{
				projectile.rotation = (float)System.Math.Atan2(projectile.velocity.X, -projectile.velocity.Y);
				Origin = projectile.position;
				projectile.velocity *= 1f/Total_Updates;
				FiringVelocity = projectile.velocity.Length();
			}
			float distanceFromStart = Vector2.DistanceSquared(projectile.position, Origin);
			if (Range*Range < distanceFromStart)
			{
				Stop();
			}
		}

		private void AIStopped()
		{
			float distanceFromOwner = Vector2.DistanceSquared(projectile.position, Main.player[projectile.owner].position);
			if (Max_Dist*Max_Dist < distanceFromOwner)
				State = State == Stopped ? FadeOut : FadeOutStuck;
		}

		private void AIReturn()
		{
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			if (Poof == 0)
			{
				Poof = 1;
				//Utils.PoofOfSmoke(projectile.position);
			}
			Vector2 velocity = Main.player[projectile.owner].MountedCenter - projectile.position;
			float distance = velocity.Length();
			if (distance < FiringVelocity)
			{
				projectile.Kill();
				return;
			}
			float startFade = 10 * Total_Updates * FiringVelocity;
			if (distance < startFade)
				projectile.alpha = 255 - (int)(distance/startFade * 255);

			velocity /= distance;
			velocity *= FiringVelocity *
				(distance < Range ?
				1.5f :
				1.5f + (distance - Range) / Range);
			projectile.velocity = velocity;
			projectile.rotation = (float)System.Math.Atan2(velocity.X, -velocity.Y) + (float)System.Math.PI;
		}

		private void AIFade()
		{
			if (projectile.numUpdates == 0)
			{
				projectile.alpha += 5;
				if (projectile.alpha >= 255)
					projectile.Kill();
			}
		}

		private void Stop()
		{
			projectile.velocity = Vector2.Zero;
			State = Stopped;
			Poof = 0;
		}


		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 0;
			height = 0;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (State != 0)
				return false;
			projectile.position += projectile.velocity *= Total_Updates;
			Stop();
			State = StuckInBlock;
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (State == Moving)
				Stop();
		}

		public override bool? CanHitNPC(NPC target)
		{
			var state = State;
			return (state == Moving || state == Return) ? null : (bool?)false;
		}

		public override bool? CanCutTiles()
		{
			return State == Moving ? null : (bool?)false;
		}

		public enum DuskfeatherState
		{
			Moving = 0,
			StuckInBlock,
			Stopped,
			Return,
			FadeOut,
			FadeOutStuck
		}
	}
}
