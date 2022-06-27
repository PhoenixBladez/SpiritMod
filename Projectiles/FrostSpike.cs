using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using static SpiritMod.Items.Glyphs.FrostGlyph;

namespace SpiritMod.Projectiles
{
	class FrostSpike : ModProjectile
	{


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spike");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.height = 12;
			Projectile.width = 12;
			Projectile.tileCollide = false;
		}

		public float Offset {
			get { return Projectile.ai[0]; }
			set { Projectile.ai[0] = value; }
		}

		public Vector2 Target =>
			new Vector2(-Projectile.ai[0], -Projectile.ai[1]);

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (player.active && Offset >= 0) {
				Projectile.penetrate = 1;
				MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
				if (player.whoAmI == Main.myPlayer && modPlayer.glyph != GlyphType.Frost) {
					Projectile.Kill();
					return;
				}

				Projectile.timeLeft = 300;
				modPlayer.frostTally++;
				int count = modPlayer.frostCount;
				float sector = MathHelper.TwoPi / (count > 0 ? count : 1);
				float rotation = modPlayer.frostRotation + Offset * sector;
				if (rotation > MathHelper.TwoPi)
					rotation -= MathHelper.TwoPi;
				float delta = Projectile.rotation;
				if (delta > MathHelper.Pi)
					delta -= MathHelper.TwoPi;
				else if (delta < -MathHelper.Pi)
					delta += MathHelper.TwoPi;
				delta = rotation - delta;
				if (delta > MathHelper.Pi)
					delta -= MathHelper.TwoPi;
				else if (delta < -MathHelper.Pi)
					delta += MathHelper.TwoPi;
				if (delta > 1.5 * TURNRATE)
					Projectile.rotation += 1.5f * TURNRATE;
				else if (delta < .5 * TURNRATE)
					Projectile.rotation += 0.5f * TURNRATE;
				else
					Projectile.rotation = rotation;
				Projectile.Center = player.MountedCenter + new Vector2(0, -OFFSET).RotatedBy(Projectile.rotation);
				return;
			}
			//else if (Offset < 0)
			//{
			//	if (!projectile.velocity.Nearing(Target - projectile.Center))
			//	{
			//		projectile.position = Target;
			//		projectile.Kill();
			//		return;
			//	}
			//}

			if (Projectile.localAI[1] == 0) {
				Projectile.localAI[1] = 1;
				ProjectileExtras.LookAlongVelocity(this);
				Projectile.penetrate = -1;
				Projectile.extraUpdates = 1;
				Projectile.tileCollide = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			if (player.active && Offset >= 0)
				player.GetModPlayer<MyPlayer>().frostUpdate = true;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[Projectile.owner];
			if (player.active && Offset >= 0)
				hitDirection = target.position.X + (target.width >> 1) - player.position.X - (player.width >> 1) > 0 ? 1 : -1;
		}
	}
}
