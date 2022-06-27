using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using System;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.StarWeaver
{
	public class WeaverStargloopChaser : ModProjectile
	{
		public override string Texture => "Terraria/Projectile_1";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Star Gloop");

		private const int MAXTIMELEFT = 160;
		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(20, 20);
			Projectile.hostile = true;
			Projectile.timeLeft = MAXTIMELEFT;
			Projectile.ignoreWater = true;
			Projectile.hide = true;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Player player = Main.player[(int)Projectile.ai[0]]; //Set when spawned

			if (player == null || !player.active || player.dead) //If target player is gone, kill projectile
			{
				Projectile.Kill();
				return;
			}

			int HomingStartDelay = 40;

			float MaxTurnSpeed = 0.075f;
			float MinTurnSpeed = 0.02f;

			float MaxSpeed = 18;
			float MinSpeed = 10;

			float progress = 1 - ((Projectile.timeLeft - HomingStartDelay) / (float)(MAXTIMELEFT - HomingStartDelay));
			progress = Math.Max(progress, 0);
			float TurnSpeed = MathHelper.Lerp(MaxTurnSpeed, MinTurnSpeed, progress);
			float Speed = MathHelper.Lerp(MinSpeed, MaxSpeed, progress);
			Projectile.velocity = Speed * Vector2.Normalize(Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.Center) * Speed, TurnSpeed));

			for (int i = 0; i < 4; i++)
				Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(i * 0.5f, i * 0.5f), ModContent.DustType<Dusts.EnemyStargoopDustFastDissipate>(), Projectile.velocity * Main.rand.NextFloat(0.5f, 1f), Scale : 2.5f);
		}
	}
}