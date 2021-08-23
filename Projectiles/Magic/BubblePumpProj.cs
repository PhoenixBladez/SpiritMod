using Microsoft.Xna.Framework;
using System;
using Terraria;
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
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.timeLeft = 999999;
		}

		int counter = 7;
		bool firing = false;
		Vector2 direction = Vector2.Zero;
		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.ChangeDir(Main.MouseWorld.X > player.position.X ? 1 : -1);
			player.heldProj = projectile.whoAmI;
			if (player.statMana <= 0) {
				projectile.Kill();
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
				projectile.position = player.Center;
				if (counter < 100) {
					counter++;
					if (counter % 20 == 19)
						Main.PlaySound(SoundID.Item5, (int)projectile.position.X, (int)projectile.position.Y);

					Vector2 dustUnit = direction.RotatedBy(Main.rand.NextFloat(-1,1)) * 0.15f;
					Vector2 dustOffset = player.Center + (direction * 5.3f) + player.velocity;
					Dust dust = Dust.NewDustPerfect(dustOffset + (dustUnit * 30), ModContent.DustType<BubbleDust>());
					dust.velocity = Vector2.Zero - (dustUnit * 1.5f);
					dust.noGravity = true;
					dust.scale = (float)Math.Sqrt(counter / 100f);

					projectile.ai[1]++;
					if (projectile.ai[1] % 20 == 19) {
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
						Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 85);
						Projectile.NewProjectile(player.Center + (direction * 5), direction.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(0.85f, 1.15f), bubbleproj, projectile.damage, projectile.knockBack, projectile.owner);
					}
				}
				else {
					projectile.active = false;
				}
			}
			return true;
		}
	}
}
