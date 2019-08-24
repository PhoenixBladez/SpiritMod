using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static SpiritMod.Items.Glyphs.FrostGlyph;

namespace SpiritMod.Projectiles
{
	class FrostSpike : ModProjectile
	{
		public static int _type;
		

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spike");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 300;
			projectile.height = 12;
			projectile.width = 12;
			projectile.tileCollide = false;
		}

		public float Offset
		{
			get { return projectile.ai[0]; }
			set { projectile.ai[0] = value; }
		}

		public Vector2 Target =>
			new Vector2(-projectile.ai[0], -projectile.ai[1]);

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (player.active && Offset >= 0)
			{
				projectile.penetrate = 1;
				MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
				if (player.whoAmI == Main.myPlayer && modPlayer.glyph != GlyphType.Frost)
				{
					projectile.Kill();
					return;
				}

				projectile.timeLeft = 300;
				modPlayer.frostTally++;
				int count = modPlayer.frostCount;
				float sector = MathHelper.TwoPi / (count > 0 ? count : 1);
				float rotation = modPlayer.frostRotation + Offset * sector;
				if (rotation > MathHelper.TwoPi)
					rotation -= MathHelper.TwoPi;
				float delta = projectile.rotation;
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
					projectile.rotation += 1.5f * TURNRATE;
				else if (delta < .5 * TURNRATE)
					projectile.rotation += 0.5f * TURNRATE;
				else
					projectile.rotation = rotation;
				projectile.Center = player.MountedCenter + new Vector2(0, -OFFSET).RotatedBy(projectile.rotation);
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

			if (projectile.localAI[1] == 0)
			{
				projectile.localAI[1] = 1;
				ProjectileExtras.LookAlongVelocity(this);
				projectile.penetrate = -1;
				projectile.extraUpdates = 1;
				projectile.tileCollide = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			if (player.active && Offset >= 0)
				player.GetModPlayer<MyPlayer>().frostUpdate = true;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[projectile.owner];
			if (player.active && Offset >= 0)
				hitDirection = target.position.X + (target.width >> 1) - player.position.X - (player.width >> 1) > 0 ? 1 : -1;
		}
	}
}
