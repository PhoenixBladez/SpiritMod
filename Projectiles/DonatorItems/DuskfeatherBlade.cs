using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using static SpiritMod.Projectiles.DonatorItems.DuskfeatherBlade.DuskfeatherState;

namespace SpiritMod.Projectiles.DonatorItems
{
	class DuskfeatherBlade : ModProjectile
	{

		private const float Range = 25 * 16;
		private const float Max_Dist = 100 * 16;
		private const int Total_Updates = 3;
		private const int Total_Lifetime = 3600 * Total_Updates;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duskfeather Blade");
			Main.projFrames[Projectile.type] = 13;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.height = 14;
			Projectile.width = 14;
			Projectile.alpha = 255;
			Projectile.penetrate = 2;
			Projectile.aiStyle = -1;
			Projectile.extraUpdates = Total_Updates - 1;
			Projectile.timeLeft = Total_Lifetime;
		}

		internal static void AttractBlades(Player player)
		{
			for (int i = 0; i < Main.maxProjectiles; ++i) {
				var projectile = Main.projectile[i];
				if (!projectile.active)
					continue;
				int state = (int)projectile.ai[0];
				if (projectile.type == ModContent.ProjectileType<DuskfeatherBlade>() &&
					projectile.owner == player.whoAmI &&
					state != (int)FadeOut &&
					state != (int)FadeOutStuck) {
					Retract(projectile);
				}
			}
		}

		internal static void AttractOldestBlade(Player player)
		{
			Projectile oldest = null;
			int timeLeft = int.MaxValue;
			for (int i = 0; i < Main.maxProjectiles; ++i) {
				var projectile = Main.projectile[i];
				if (!projectile.active)
					continue;
				int state = (int)projectile.ai[0];
				if (projectile.type == ModContent.ProjectileType<DuskfeatherBlade>() &&
					projectile.owner == player.whoAmI &&
					state != (int)Return &&
					state != (int)FadeOut &&
					state != (int)FadeOutStuck &&
					projectile.timeLeft < timeLeft) {
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
			if (Projectile.alpha == 255)
				return;
		}

		private DuskfeatherState State {
			get { return (DuskfeatherState)(int)Projectile.ai[0]; }
			set { Projectile.ai[0] = (int)value; }
		}
		private float FiringVelocity {
			get { return Projectile.ai[1]; }
			set { Projectile.ai[1] = value; }
		}
		private Vector2 Origin {
			get { return new Vector2(Projectile.localAI[0], Projectile.localAI[1]); }
			set {
				Projectile.localAI[0] = value.X;
				Projectile.localAI[1] = value.Y;
			}
		}
		private float Poof {
			get { return Projectile.localAI[0]; }
			set { Projectile.localAI[0] = value; }
		}
		public override void AI()
		{
			if (State < Return) {
				if (Projectile.alpha > 25)
					Projectile.alpha -= 25;
				else
					Projectile.alpha = 0;
			}
			int minFrame = 7;
			int maxFrame = 12;
			switch (State) {
				case Moving:
					AIMove();
					break;
				case StuckInBlock:
					maxFrame = 7;
					AIStopped();
					break;
				case DuskfeatherState.Stopped:
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
			if (Projectile.numUpdates == 0) {
				if (State == Moving || State == Return)
					++Projectile.frameCounter;
				if (++Projectile.frameCounter >= 5) {
					Projectile.frameCounter = 0;
					++Projectile.frame;
				}
				if (Projectile.frame < minFrame || Projectile.frame > maxFrame)
					Projectile.frame = minFrame;
			}
		}

		private void AIMove()
		{
			if (Origin == Vector2.Zero) {
				Projectile.rotation = (float)System.Math.Atan2(Projectile.velocity.X, -Projectile.velocity.Y);
				Origin = Projectile.position;
				Projectile.velocity *= 1f / Total_Updates;
				FiringVelocity = Projectile.velocity.Length();
			}
			float distanceFromStart = Vector2.DistanceSquared(Projectile.position, Origin);
			if (Range * Range < distanceFromStart) {
				Stop();
			}
		}

		private void AIStopped()
		{
			float distanceFromOwner = Vector2.DistanceSquared(Projectile.position, Main.player[Projectile.owner].position);
			if (Max_Dist * Max_Dist < distanceFromOwner)
				State = State == DuskfeatherState.Stopped ? FadeOut : FadeOutStuck;
		}

		private void AIReturn()
		{
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			if (Poof == 0) {
				Poof = 1;
				//Utils.PoofOfSmoke(projectile.position);
			}
			Vector2 velocity = Main.player[Projectile.owner].MountedCenter - Projectile.position;
			float distance = velocity.Length();
			if (distance < FiringVelocity) {
				Projectile.Kill();
				return;
			}
			float startFade = 10 * Total_Updates * FiringVelocity;
			if (distance < startFade)
				Projectile.alpha = 255 - (int)(distance / startFade * 255);

			velocity /= distance;
			velocity *= FiringVelocity *
				(distance < Range ?
				1.5f :
				1.5f + (distance - Range) / Range);
			Projectile.velocity = velocity;
			Projectile.rotation = (float)System.Math.Atan2(velocity.X, -velocity.Y) + (float)System.Math.PI;
		}

		private void AIFade()
		{
			if (Projectile.numUpdates == 0) {
				Projectile.alpha += 5;
				if (Projectile.alpha >= 255)
					Projectile.Kill();
			}
		}

		private void Stop()
		{
			Projectile.velocity = Vector2.Zero;
			State = DuskfeatherState.Stopped;
			Poof = 0;
		}


		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 0;
			height = 0;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (State != 0)
				return false;
			Projectile.position += Projectile.velocity *= Total_Updates;
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
