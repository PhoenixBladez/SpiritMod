using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.DonatorItems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class BoughSeed : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Seed");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;

		}
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 20;
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.light = 0;
			Projectile.timeLeft = 300;
			Projectile.extraUpdates = 1;
		}

		public override bool PreAI()
		{
			if (Projectile.ai[0] == 0) {
				Projectile.rotation = Projectile.velocity.ToRotation() + 1.57F;
			}
			else {
				Projectile.ignoreWater = true;
				Projectile.tileCollide = false;
				int num996 = 1;
				bool flag52 = false;
				bool flag53 = false;
				Projectile.localAI[0] += 1f;
				if (Projectile.localAI[0] % 30f == 0f) {
					flag53 = true;
				}
				int num997 = (int)Projectile.ai[1];
				if (Projectile.localAI[0] >= (float)(60 * num996)) {
					flag52 = true;
				}
				else if (num997 < 0 || num997 >= 300) {
					flag52 = true;
				}
				else if (Main.npc[num997].active && !Main.npc[num997].dontTakeDamage) {
					Projectile.Center = Main.npc[num997].Center - Projectile.velocity * 2f;
					Projectile.gfxOffY = Main.npc[num997].gfxOffY;
					if (flag53) {
						Main.npc[num997].HitEffect(0, 1.0);
					}
				}
				else {
					flag52 = true;
				}
				if (flag52) {
					Projectile.Kill();
				}
			}

			return false;
		}
		public override void AI()
		{
			Vector2 targetPos = Projectile.Center;
			float targetDist = 350f;
			bool targetAcquired = false;

			//loop through first 200 NPCs in Main.npc
			//this loop finds the closest valid target NPC within the range of targetDist pixels
			for (int i = 0; i < 200; i++) {
				if (Main.npc[i].CanBeChasedBy(Projectile) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1)) {
					float dist = Projectile.Distance(Main.npc[i].Center);
					if (dist < targetDist) {
						targetDist = dist;
						targetPos = Main.npc[i].Center;
						targetAcquired = true;
					}
				}
			}

			//change trajectory to home in on target
			if (targetAcquired) {
				float homingSpeedFactor = 6f;
				Vector2 homingVect = targetPos - Projectile.Center;
				float dist = Projectile.Distance(targetPos);
				dist = homingSpeedFactor / dist;
				homingVect *= dist;

				Projectile.velocity = (Projectile.velocity * 20 + homingVect) / 21f;
			}

			//Spawn the dust
			if (Main.rand.Next(11) == 0) {
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.RedTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + (float)(Math.PI / 2);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.ai[0] = 1f;
			Projectile.ai[1] = (float)target.whoAmI;
			Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
			Projectile.netUpdate = true;
			{
				Projectile.damage = 0;
			}
			knockback = 0;
			int num31 = 6;
			Point[] array2 = new Point[num31];
			int num32 = 0;

			for (int n = 0; n < 1000; n++) {
				if (n != Projectile.whoAmI && Main.projectile[n].active && Main.projectile[n].owner == Main.myPlayer && Main.projectile[n].type == Projectile.type && Main.projectile[n].ai[0] == 1f && Main.projectile[n].ai[1] == target.whoAmI) {
					array2[num32++] = new Point(n, Main.projectile[n].timeLeft);
					if (num32 >= array2.Length) {
						break;
					}
				}
			}
			if (num32 >= array2.Length) {
				int num33 = 0;
				for (int num34 = 1; num34 < array2.Length; num34++) {
					if (array2[num34].Y < array2[num33].Y) {
						num33 = num34;
					}
				}
				Main.projectile[array2[num33].X].Kill();


			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			{
				Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<Wrath>(), 55, 7, Main.myPlayer);

				for (int num621 = 0; num621 < 40; num621++) {
					int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.RedTorch, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 1.2f;
					Main.dust[num622].noGravity = true;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
			SoundEngine.PlaySound(SoundID.Dig, (int)Projectile.position.X, (int)Projectile.position.Y);
		}
	}
}
