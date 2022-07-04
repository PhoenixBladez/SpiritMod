using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	public class LargeScarab : ModProjectile
	{

		public override void SetStaticDefaults() { 
			DisplayName.SetDefault("Scarab");
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 360;
			Projectile.frame = Main.rand.Next(4);
			Projectile.alpha = 255;
		}

		public override bool CanHitPlayer(Player target) => Projectile.ai[1] >= 120;
		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height); 
			for (int i = 0; i < 10; i++) {
				int d = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Plantera_Green, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
				Main.dust[d].noGravity = true;
				Main.dust[d].scale = 1.2f;
			}
			SoundEngine.PlaySound(SoundID.NPCDeath16, Projectile.Center);
			for (int i = 1; i <= 3; i++) {
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, Projectile.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Scarabeus/largescarab" + i.ToString()).Type);
			}
		}

		public override void AI()
		{
			Player player = Main.player[(int)Projectile.ai[0]];
			if (!player.active || player.dead || !NPC.AnyNPCs(ModContent.NPCType<Scarabeus>()))
				Projectile.Kill();
			if (Projectile.alpha > 0)
				Projectile.alpha -= 5;
			else
				Projectile.alpha = 0;
			Projectile.ai[1]++;
			if(Projectile.ai[1] < 130) {
				Projectile.spriteDirection = Projectile.direction;
				Vector2 homepos = player.Center;
				homepos.Y -= 180;
				Projectile.gfxOffY = Math.Sign(Projectile.ai[1] / 5) * 25;
				float vel = MathHelper.Clamp(Projectile.Distance(homepos) / 20, 3, 18);
				Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, Projectile.DirectionTo(homepos).Y * vel, 0.05f);
				Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, 0, 0.07f);
				if (Math.Abs(Projectile.velocity.X) <= 2f || Math.Abs(player.Center.X - Projectile.Center.X) > 380) {
					Projectile.velocity.X = Math.Sign(Projectile.DirectionTo(homepos).X) * MathHelper.Clamp(Math.Abs((player.Center.X - Projectile.Center.X) / 12), 19, 28) * Main.rand.NextFloat(1f, 1.2f);
					Projectile.netUpdate = true;
				}
			}
			else if(Projectile.ai[1] < 170) {
				Projectile.gfxOffY = MathHelper.Lerp(Projectile.gfxOffY, 0, 0.1f);
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, -Vector2.UnitY * 5, 0.15f);
				if(Projectile.ai[1] == 130)
					SoundEngine.PlaySound(SoundID.Zombie44, Projectile.Center);
			}
			else if (Projectile.ai[1] == 170) {
				Projectile.spriteDirection = Projectile.direction;
				Projectile.tileCollide = (Projectile.Center.Y >= player.position.Y);
				Projectile.velocity = Vector2.Lerp(Projectile.DirectionTo(player.Center), Vector2.UnitY, 0.25f) * 8;
				Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);
			}
			else {
				Projectile.velocity.Y *= 1.03f;
				Projectile.tileCollide = (Projectile.Center.Y >= player.position.Y);
			}

			Projectile.frameCounter++;
			if(Projectile.frameCounter > 6) {
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.frame = 0;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if(Projectile.ai[1] > 170)
				Projectile.QuickDrawTrail(Main.spriteBatch);

			Projectile.QuickDraw(Main.spriteBatch);
			return false;
		}
	}
}