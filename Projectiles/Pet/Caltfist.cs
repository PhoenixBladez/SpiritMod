using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Pet
{
	public class Caltfist : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caltfist");
			Main.projFrames[projectile.type] = 1;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ZephyrFish);
			aiType = ProjectileID.ZephyrFish;
			projectile.width = 40;
			projectile.height = 60;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.zephyrfish = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.75f) / 255f, ((255 - projectile.alpha) * 0.75f) / 255f, ((255 - projectile.alpha) * 0f) / 255f);
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.dead)
				modPlayer.caltfist = false;

			if (modPlayer.caltfist)
				projectile.timeLeft = 2;
			
			projectile.localAI[0] += 1f;
					if (projectile.localAI[0] >= 10f)
					{
						projectile.localAI[0] = 0f;
						int num171 = 30;
						if ((projectile.Center - Main.player[Main.myPlayer].Center).Length() < (float)(Main.screenWidth + num171 * 16))
						{
							int num172 = (int)projectile.Center.X / 16;
							int num173 = (int)projectile.Center.Y / 16;
							int num3;
							for (int num174 = num172 - num171; num174 <= num172 + num171; num174 = num3 + 1)
							{
								for (int num175 = num173 - num171; num175 <= num173 + num171; num175 = num3 + 1)
								{
									if (Main.rand.Next(4) == 0)
									{
										Vector2 vector16 = new Vector2((float)(num172 - num174), (float)(num173 - num175));
										if (vector16.Length() < (float)num171 && num174 > 0 && num174 < Main.maxTilesX - 1 && num175 > 0 && num175 < Main.maxTilesY - 1 && Main.tile[num174, num175] != null && Main.tile[num174, num175].active())
										{
											bool flag3 = false;
											if (Main.tile[num174, num175].type == 185 && Main.tile[num174, num175].frameY == 18)
											{
												if (Main.tile[num174, num175].frameX >= 576 && Main.tile[num174, num175].frameX <= 882)
												{
													flag3 = true;
												}
											}
											else if (Main.tile[num174, num175].type == 186 && Main.tile[num174, num175].frameX >= 864 && Main.tile[num174, num175].frameX <= 1170)
											{
												flag3 = true;
											}
											if (flag3 || Main.tileSpelunker[(int)Main.tile[num174, num175].type] || (Main.tileAlch[(int)Main.tile[num174, num175].type] && Main.tile[num174, num175].type != 82))
											{
												int num176 = Dust.NewDust(new Vector2((float)(num174 * 16), (float)(num175 * 16)), 16, 16, 204, 0f, 0f, 150, default(Color), 0.3f);
												Main.dust[num176].fadeIn = 0.75f;
												Dust dust3 = Main.dust[num176];
												dust3.velocity *= 0.1f;
												Main.dust[num176].noLight = true;
											}
										}
									}
									num3 = num175;
								}
								num3 = num174;
							}
						}
					}
		}

	}
}