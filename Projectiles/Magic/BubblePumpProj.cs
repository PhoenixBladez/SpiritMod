using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using SpiritMod.Dusts;
using Terraria.ID;

namespace SpiritMod.Projectiles.Magic
{
	public class BubblePumpProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.timeLeft = 999999;
		}

		int counter = 7;
		bool firing = false;
		Vector2 direction = Vector2.Zero;
		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.ChangeDir(Main.MouseWorld.X > player.position.X ? 1 : -1);
			player.heldProj = Projectile.whoAmI;
			if (player.statMana <= 0) {
				Projectile.Kill();
			}

			direction = Main.MouseWorld - (player.Center - new Vector2(4, 4));
			direction.Normalize();
			direction *= 7f;
			player.itemTime = 5;
			player.itemAnimation = 5;
			player.velocity.X *= 0.97f;
			if (counter == 7) {
				if (player.statMana > 0) {
                    player.statMana -= 20;
					player.manaRegenDelay = 60;
				}
				else {
					firing = true;
				}
			}
			if (player.channel && !firing) {
				Projectile.position = player.Center;
				if (counter < 100) {
					counter++;
					if (counter % 20 == 19)
						SoundEngine.PlaySound(SoundID.Item5, (int)Projectile.position.X, (int)Projectile.position.Y);

					Vector2 dustUnit = direction.RotatedBy(Main.rand.NextFloat(-1,1)) * 0.15f;
					Vector2 dustOffset = player.Center + (direction * 5.3f) + player.velocity;
					Dust dust = Dust.NewDustPerfect(dustOffset + (dustUnit * 30), ModContent.DustType<BubbleDust>());
					dust.velocity = Vector2.Zero - (dustUnit * 1.5f);
					dust.noGravity = true;
					dust.scale = (float)Math.Sqrt(counter / 100f);

					Projectile.ai[1]++;
					if (Projectile.ai[1] % 20 == 19) {
						if (player.statMana > 0) {
							player.statMana -= 20;
							player.manaRegenDelay = 90;
						}
						else {
							firing = true;
						}
					}
				}
				if (counter == 100) {
					counter = 130;
				}
				direction = direction.RotatedBy(Main.rand.NextFloat(0 - ((float)Math.Sqrt(counter) / 120f), ((float)Math.Sqrt(counter) / 120f)));
				player.itemRotation = direction.ToRotation();
				if (player.direction != 1)
				{
					player.itemRotation -= 3.14f;
				}
			}
			else {
				firing = true;
				if (counter > 0) {
					counter -= 2;
					if (counter % 5 == 0) {
						direction = direction.RotatedBy(Main.rand.NextFloat(-0.18f, 0.18f));
						player.itemRotation = direction.ToRotation();
						if (player.direction != 1)
						{
							player.itemRotation -= 3.14f;
						}
						int bubbleproj;
						bubbleproj = Main.rand.Next(new int[] {
							ModContent.ProjectileType<GunBubble1>(),
							ModContent.ProjectileType<GunBubble2>(),
							ModContent.ProjectileType<GunBubble3>(),
							ModContent.ProjectileType<GunBubble4>(),
							ModContent.ProjectileType<GunBubble5>()
						});
						SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 85);
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center + (direction * 5), direction.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(0.85f, 1.15f), bubbleproj, Projectile.damage, Projectile.knockBack, Projectile.owner);
					}
				}
				else {
					Projectile.active = false;
				}
			}
			return true;
		}
	}
}
