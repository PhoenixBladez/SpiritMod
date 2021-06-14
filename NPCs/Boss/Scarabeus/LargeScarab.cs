using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	public class LargeScarab : ModProjectile
	{

		public override void SetStaticDefaults() { 
			DisplayName.SetDefault("Sand Ball");
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 36;
			projectile.height = 36;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.timeLeft = 360;
			projectile.frame = Main.rand.Next(4);
			projectile.alpha = 255;
		}

		public override bool CanHitPlayer(Player target) => projectile.ai[1] >= 120;
		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height); 
			for (int i = 0; i < 10; i++) {
				int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 167, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
				Main.dust[d].noGravity = true;
				Main.dust[d].scale = 1.2f;
			}
			Main.PlaySound(SoundID.NPCDeath16, (int)projectile.Center.X, (int)projectile.Center.Y);
			for (int i = 1; i <= 3; i++) {
				Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/Scarabeus/largescarab" + i.ToString()));
			}
		}

		public override void AI()
		{
			Player player = Main.player[(int)projectile.ai[0]];
			if (!player.active || player.dead || !NPC.AnyNPCs(ModContent.NPCType<Scarabeus>()))
				projectile.Kill();
			if (projectile.alpha > 0)
				projectile.alpha -= 5;
			else
				projectile.alpha = 0;
			projectile.ai[1]++;
			if(projectile.ai[1] < 130) {
				projectile.spriteDirection = projectile.direction;
				Vector2 homepos = player.Center;
				homepos.Y -= 180;
				projectile.gfxOffY = Math.Sign(projectile.ai[1] / 5) * 25;
				float vel = MathHelper.Clamp(projectile.Distance(homepos) / 20, 3, 18);
				projectile.velocity.Y = MathHelper.Lerp(projectile.velocity.Y, projectile.DirectionTo(homepos).Y * vel, 0.05f);
				projectile.velocity.X = MathHelper.Lerp(projectile.velocity.X, 0, 0.07f);
				if (Math.Abs(projectile.velocity.X) <= 2f || Math.Abs(player.Center.X - projectile.Center.X) > 380) {
					projectile.velocity.X = Math.Sign(projectile.DirectionTo(homepos).X) * MathHelper.Clamp(Math.Abs((player.Center.X - projectile.Center.X) / 12), 19, 28) * Main.rand.NextFloat(1f, 1.2f);
					projectile.netUpdate = true;
				}
			}
			else if(projectile.ai[1] < 170) {
				projectile.gfxOffY = MathHelper.Lerp(projectile.gfxOffY, 0, 0.1f);
				projectile.velocity = Vector2.Lerp(projectile.velocity, -Vector2.UnitY * 5, 0.15f);
				if(projectile.ai[1] == 130)
					Main.PlaySound(SoundID.Zombie, (int)projectile.position.X, (int)projectile.position.Y, 44);
			}
			else if (projectile.ai[1] == 170) {
				projectile.spriteDirection = projectile.direction;
				projectile.tileCollide = (projectile.Center.Y >= player.position.Y);
				projectile.velocity = Vector2.Lerp(projectile.DirectionTo(player.Center), Vector2.UnitY, 0.25f) * 8;
				projectile.spriteDirection = -Math.Sign(projectile.velocity.X);
			}
			else {
				projectile.velocity.Y *= 1.03f;
				projectile.tileCollide = (projectile.Center.Y >= player.position.Y);
			}

			projectile.frameCounter++;
			if(projectile.frameCounter > 6) {
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
					projectile.frame = 0;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if(projectile.ai[1] > 170)
				projectile.QuickDrawTrail(spriteBatch);

			projectile.QuickDraw(spriteBatch);
			return false;
		}
	}
}