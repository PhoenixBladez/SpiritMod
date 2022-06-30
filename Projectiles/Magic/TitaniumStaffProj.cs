using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.MagicMisc.HardmodeOreStaves;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
			Projectile.height = 14;
			Projectile.width = 26;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}

		public float Offset
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public Vector2 Target =>
			new Vector2(-Projectile.ai[0], -Projectile.ai[1]);

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.alpha = Math.Max(Projectile.alpha - 8, 0);
			if (Main.player[Projectile.owner].channel)
			{
				Projectile.penetrate = 1;
				Projectile.timeLeft -= 2;
				if (Projectile.timeLeft <= 170)
				{
					Projectile.tileCollide = true;
				}
				if (Projectile.timeLeft <= 0)
				{
					Projectile.Kill();
					player.GetSpiritPlayer().shadowCount = 0;
				}
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
				float num2353 = 16f;
				Vector2 vector329 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				if (Main.myPlayer == Projectile.owner)
				{
					float num2352 = (float)Main.mouseX + Main.screenPosition.X - vector329.X;
					float num2351 = (float)Main.mouseY + Main.screenPosition.Y - vector329.Y;
					if (Main.player[Projectile.owner].gravDir == -1f)
					{
						num2351 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector329.Y;
					}
					float num2350 = (float)Math.Sqrt((double)(num2352 * num2352 + num2351 * num2351));
					num2350 = (float)Math.Sqrt((double)(num2352 * num2352 + num2351 * num2351));
					if (Projectile.ai[0] < 0f)
					{
						Projectile.ai[0] += 1f;
					}
					if (Projectile.type == 491 && num2350 < 100f)
					{
						if (Projectile.velocity.Length() < num2353)
						{
							Projectile.velocity *= 1.1f;
							if (Projectile.velocity.Length() > num2353)
							{
								Projectile.velocity.Normalize();
								Projectile.velocity *= num2353;
							}
						}
						if (Projectile.ai[0] == 0f)
						{
							Projectile.ai[0] = -10f;
						}
					}
					else if (num2350 > num2353)
					{
						num2350 = num2353 / num2350;
						num2352 *= num2350;
						num2351 *= num2350;
						int num2345 = (int)(num2352 * 1000f);
						int num2344 = (int)(Projectile.velocity.X * 1000f);
						int num2343 = (int)(num2351 * 1000f);
						int num2342 = (int)(Projectile.velocity.Y * 1000f);
						if (num2345 != num2344 || num2343 != num2342)
						{
							Projectile.netUpdate = true;
						}
						if (Projectile.type == 491)
						{
							Vector2 value167 = new Vector2(num2352, num2351);
							Projectile.velocity = (Projectile.velocity * 4f + value167) / 5f;
						}
						else
						{
							Projectile.velocity.X = num2352;
							Projectile.velocity.Y = num2351;
						}
					}
					else
					{
						int num2341 = (int)(num2352 * 1000f);
						int num2340 = (int)(Projectile.velocity.X * 1000f);
						int num2339 = (int)(num2351 * 1000f);
						int num2338 = (int)(Projectile.velocity.Y * 1000f);
						if (num2341 != num2340 || num2339 != num2338)
						{
							Projectile.netUpdate = true;
						}
						Projectile.velocity.X = num2352;
						Projectile.velocity.Y = num2351;
					}
				}

			}
			else if (Projectile.ai[0] <= 0f)
			{
				Projectile.netUpdate = true;
				if (Projectile.type != 491)
				{
					float num2337 = 12f;
					Vector2 vector328 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
					float num2336 = (float)Main.mouseX + Main.screenPosition.X - vector328.X;
					float num2335 = (float)Main.mouseY + Main.screenPosition.Y - vector328.Y;
					if (Main.player[Projectile.owner].gravDir == -1f)
					{
						num2335 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector328.Y;
					}
					float num2334 = (float)Math.Sqrt((double)(num2336 * num2336 + num2335 * num2335));
					if (num2334 == 0f || Projectile.ai[0] < 0f)
					{
						vector328 = new Vector2(Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2), Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2));
						num2336 = Projectile.position.X + (float)Projectile.width * 0.5f - vector328.X;
						num2335 = Projectile.position.Y + (float)Projectile.height * 0.5f - vector328.Y;
						num2334 = (float)Math.Sqrt((double)(num2336 * num2336 + num2335 * num2335));
					}
					num2334 = num2337 / num2334;
					num2336 *= num2334;
					num2335 *= num2334;
					Projectile.velocity.X = num2336;
					Projectile.velocity.Y = num2335;
					if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
						Projectile.Kill();
				}
				Projectile.ai[0] = 1f;
			}
			else if (player.active && Offset >= 0)
			{
				Projectile.tileCollide = false;
				Projectile.penetrate = 1;
				MyPlayer modPlayer = player.GetSpiritPlayer();
				if (player.whoAmI == Main.myPlayer && player.inventory[player.selectedItem].type != ModContent.ItemType<TitaniumStaff>())
				{
					Projectile.Kill();
					player.GetSpiritPlayer().shadowCount = 0;
					return;
				}

				Projectile.timeLeft = 300;
				double deg = (double)Projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
				double rad = deg * (Math.PI / 180); //Convert degrees to radians

				/*Position the projectile based on where the player is, the Sin/Cos of the angle times the /
				/distance for the desired distance away from the player minus the projectile's width   /
				/and height divided by two so the center of the projectile is at the right place.     */

				//Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
				Projectile.ai[1] += .8f;
				double dist = 80;
				Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
				Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
				Vector2 direction = Vector2.Normalize(player.Center - Projectile.Center);
				Projectile.rotation = Projectile.DirectionTo(Main.MouseWorld).ToRotation() + 1.57f;
				return;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SilverCoin, 0f, -2f, 0, default, .9f);
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X = dust.position.X + ((float)(Main.rand.Next(-10, 11) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-10, 11) / 20) - 1.5f);
				if (Main.dust[num].position != Projectile.Center)
				{
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 2f;
				}
			}

			Player player = Main.player[Projectile.owner];
			if (player.active && Offset >= 0)
				player.GetModPlayer<MyPlayer>().shadowCount--;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.tileCollide)
				SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 3);
			return true;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[Projectile.owner];
			if (player.active && Offset >= 0)
				hitDirection = target.position.X + (target.width >> 1) - player.position.X - (player.width >> 1) > 0 ? 1 : -1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 7;
			if (Main.rand.Next(3) == 0)
			{
				int randX = target.position.X > Main.player[Projectile.owner].position.X ? 1 : -1;
				int randY = Main.rand.Next(2) == 0 ? -1 : 1;

				Vector2 randPos = target.Center + new Vector2(randX * Main.rand.Next(100, 151), randY * 400);
				Vector2 dir = target.Center - randPos;
				dir.Normalize();
				dir *= 14;
				int newProj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), randPos.X, randPos.Y, dir.X, dir.Y, ModContent.ProjectileType<TitaniumStaffProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1);
				Main.projectile[newProj].tileCollide = false;
				Main.projectile[newProj].penetrate = 1;
				Main.projectile[newProj].timeLeft = 40;
				Main.projectile[newProj].extraUpdates = 1;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(100, 100, 100, 100) * Projectile.Opacity;
	}
}
