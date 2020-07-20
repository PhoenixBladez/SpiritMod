using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Magic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class TitaniumStaffProj : ModProjectile
	{
		public const float TURNRATE = (float)(0.4 * Math.PI / 30d);
		public const float OFFSET = 50;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Honed Sword");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 180;
			projectile.height = 14;
			projectile.width = 26;
			projectile.tileCollide = false;
		}

		public float Offset {
			get { return projectile.ai[0]; }
			set { projectile.ai[0] = value; }
		}

		public Vector2 Target =>
			new Vector2(-projectile.ai[0], -projectile.ai[1]);

		public override void AI()
		{
			if (Main.myPlayer == projectile.owner) 
			{
				projectile.netUpdate = true;
			}
			Player player = Main.player[projectile.owner];
			if(Main.player[projectile.owner].channel) {
				projectile.penetrate = 1;
				projectile.timeLeft -= 2;
				if(projectile.timeLeft <= 170) {
					projectile.tileCollide = true;
				}
				if(projectile.timeLeft <= 0) {
					projectile.Kill();
					player.GetSpiritPlayer().shadowCount = 0;
				}
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
				float num2353 = 16f;
				Vector2 vector329 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num2352 = (float)Main.mouseX + Main.screenPosition.X - vector329.X;
				float num2351 = (float)Main.mouseY + Main.screenPosition.Y - vector329.Y;
				if(Main.player[projectile.owner].gravDir == -1f) {
					num2351 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector329.Y;
				}
				float num2350 = (float)Math.Sqrt((double)(num2352 * num2352 + num2351 * num2351));
				num2350 = (float)Math.Sqrt((double)(num2352 * num2352 + num2351 * num2351));
				if(projectile.ai[0] < 0f) {
					projectile.ai[0] += 1f;
				}
				if(projectile.type == 491 && num2350 < 100f) {
					if(projectile.velocity.Length() < num2353) {
						projectile.velocity *= 1.1f;
						if(projectile.velocity.Length() > num2353) {
							projectile.velocity.Normalize();
							projectile.velocity *= num2353;
						}
					}
					if(projectile.ai[0] == 0f) {
						projectile.ai[0] = -10f;
					}
				} else if(num2350 > num2353) {
					num2350 = num2353 / num2350;
					num2352 *= num2350;
					num2351 *= num2350;
					int num2345 = (int)(num2352 * 1000f);
					int num2344 = (int)(projectile.velocity.X * 1000f);
					int num2343 = (int)(num2351 * 1000f);
					int num2342 = (int)(projectile.velocity.Y * 1000f);
					if(num2345 != num2344 || num2343 != num2342) {
						projectile.netUpdate = true;
					}
					if(projectile.type == 491) {
						Vector2 value167 = new Vector2(num2352, num2351);
						projectile.velocity = (projectile.velocity * 4f + value167) / 5f;
					} else {
						projectile.velocity.X = num2352;
						projectile.velocity.Y = num2351;
					}
				} else {
					int num2341 = (int)(num2352 * 1000f);
					int num2340 = (int)(projectile.velocity.X * 1000f);
					int num2339 = (int)(num2351 * 1000f);
					int num2338 = (int)(projectile.velocity.Y * 1000f);
					if(num2341 != num2340 || num2339 != num2338) {
						projectile.netUpdate = true;
					}
					projectile.velocity.X = num2352;
					projectile.velocity.Y = num2351;
				}
			} else if(projectile.ai[0] <= 0f) {
				projectile.netUpdate = true;
				if(projectile.type != 491) {
					float num2337 = 12f;
					Vector2 vector328 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num2336 = (float)Main.mouseX + Main.screenPosition.X - vector328.X;
					float num2335 = (float)Main.mouseY + Main.screenPosition.Y - vector328.Y;
					if(Main.player[projectile.owner].gravDir == -1f) {
						num2335 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector328.Y;
					}
					float num2334 = (float)Math.Sqrt((double)(num2336 * num2336 + num2335 * num2335));
					if(num2334 == 0f || projectile.ai[0] < 0f) {
						vector328 = new Vector2(Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2), Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2));
						num2336 = projectile.position.X + (float)projectile.width * 0.5f - vector328.X;
						num2335 = projectile.position.Y + (float)projectile.height * 0.5f - vector328.Y;
						num2334 = (float)Math.Sqrt((double)(num2336 * num2336 + num2335 * num2335));
					}
					num2334 = num2337 / num2334;
					num2336 *= num2334;
					num2335 *= num2334;
					projectile.velocity.X = num2336;
					projectile.velocity.Y = num2335;
					if(projectile.velocity.X == 0f && projectile.velocity.Y == 0f) {
						projectile.Kill();
					}
				}
				projectile.ai[0] = 1f;
			} else if(player.active && Offset >= 0) {
				projectile.tileCollide = false;
				projectile.penetrate = 1;
				MyPlayer modPlayer = player.GetSpiritPlayer();
				if(player.whoAmI == Main.myPlayer && player.inventory[player.selectedItem].type != ModContent.ItemType<TitaniumStaff>()) {
					projectile.Kill();
					player.GetSpiritPlayer().shadowCount = 0;
					return;
				}

				projectile.timeLeft = 300;
				double deg = (double)projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
				double rad = deg * (Math.PI / 180); //Convert degrees to radians

				/*Position the projectile based on where the player is, the Sin/Cos of the angle times the /
				/distance for the desired distance away from the player minus the projectile's width   /
				/and height divided by two so the center of the projectile is at the right place.     */

				//Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
				projectile.ai[1] += .8f;
				double dist = 80;
				Vector2 direction = Vector2.Zero;
				projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width / 2;
				projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height / 2;
				direction = player.Center - projectile.Center;
				direction.Normalize();
				projectile.rotation = projectile.DirectionTo(Main.MouseWorld).ToRotation() + 1.57f;
				return;
			}
		}

		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 10; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SilverCoin, 0f, -2f, 0, default(Color), .9f);
				Main.dust[num].noGravity = true;
				Dust expr_62_cp_0 = Main.dust[num];
				expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-10, 11) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-10, 11) / 20) - 1.5f);
				if(Main.dust[num].position != projectile.Center) {
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 2f;
				}
			}
			Player player = Main.player[projectile.owner];
			if(player.active && Offset >= 0)
				player.GetModPlayer<MyPlayer>().shadowCount--;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if(projectile.tileCollide == true) {
				Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			}
			return true;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[projectile.owner];
			if(player.active && Offset >= 0)
				hitDirection = target.position.X + (target.width >> 1) - player.position.X - (player.width >> 1) > 0 ? 1 : -1;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 7;
			if(Main.rand.Next(3) == 0) {
				int randX = target.position.X > Main.player[projectile.owner].position.X ? 1 : -1;
				int randY = Main.rand.Next(2) == 0 ? -1 : 1;

				Vector2 randPos = target.Center + new Vector2(randX * Main.rand.Next(100, 151), randY * 400);
				Vector2 dir = target.Center - randPos;
				dir.Normalize();
				dir *= 14;
				int newProj = Projectile.NewProjectile(randPos.X, randPos.Y, dir.X, dir.Y, mod.ProjectileType("TitaniumStaffProj2"), projectile.damage, projectile.knockBack, projectile.owner, 1);
				Main.projectile[newProj].tileCollide = false;
				Main.projectile[newProj].penetrate = 1;
				Main.projectile[newProj].timeLeft = 40;
				Main.projectile[newProj].extraUpdates = 1;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for(int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(100, 100, 100, 100);
		}
	}
}
