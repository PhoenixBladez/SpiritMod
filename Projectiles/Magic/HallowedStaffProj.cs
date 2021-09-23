using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class HallowedStaffProj : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Mageblade");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 10;       //projectile width
			projectile.height = 20;  //projectile height
			projectile.friendly = true;      //make that the projectile will not damage you
			projectile.magic = true;         // 
			projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
			projectile.penetrate = 2;      //how many npc will penetrate
			projectile.timeLeft = 390;   //how many time projectile projectile has before disepire // projectile light
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;
			projectile.aiStyle = -1;
		}

		int timer;
		int colortimer;

		public void DoTrailCreation(TrailManager tManager)
		{
			switch (Main.rand.Next(3)) {
				case 0:
					tManager.CreateTrail(projectile, new StandardColorTrail(new Color(115, 220, 255)), new RoundCap(), new SleepingStarTrailPosition(), 8f, 100f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
					break;
				case 1:
					tManager.CreateTrail(projectile, new StandardColorTrail(new Color(255, 231, 145)), new RoundCap(), new SleepingStarTrailPosition(), 8f, 100f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
					break;
				case 2:
					tManager.CreateTrail(projectile, new StandardColorTrail(new Color(255, 128, 244)), new RoundCap(), new SleepingStarTrailPosition(), 8f, 100f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
					break;
			}
		}

		public override void AI()
		{
			timer++;

			if (timer <= 50)
				colortimer++;
			else if (timer > 50)
				colortimer--;

			if (timer >= 100)
				timer = 0;

			float num395 = (Main.mouseTextColor / 155f - 0.35f) * 0.34f;
			projectile.scale = num395 + 0.55f;
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			Lighting.AddLight((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), 0.396f / 2, 0.370588235f / 2, 0.364705882f / 2);

			int target = -1;

			for (int i = 0; i < Main.maxNPCs; i++) {
				if (Main.npc[i].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1)) {
					float num23 = Main.npc[i].position.X + (Main.npc[i].width / 2);
					float num24 = Main.npc[i].position.Y + (Main.npc[i].height / 2);
					float num25 = Math.Abs(projectile.Center.X - num23) + Math.Abs(projectile.Center.Y - num24);
					if (num25 < 500f) {
						target = i;
						break;
					}
				}
			}

			if (target != -1) {
				const float Speed = 12.5f;
				
				Vector2 direction5 = Vector2.Normalize(Main.npc[target].Center - projectile.Center);
				projectile.rotation = projectile.DirectionTo(Main.npc[target].Center).ToRotation() + 1.57f;

				if (Main.rand.Next(16) == 0) {
					projectile.velocity = direction5 * Speed;
				}
			}

			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 7200f) {
				projectile.alpha += 5;
				if (projectile.alpha > 255) {
					projectile.alpha = 255;
					projectile.Kill();
				}
			}

			projectile.localAI[0] += 1f;

			if (projectile.localAI[0] >= 10f) {
				projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
						num416++;
						if (Main.projectile[num420].ai[1] > num418) {
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
					if (num416 > 8) {
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						return;
					}
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(60 + colortimer, 60 + colortimer, 60 + colortimer, 100);
	}
}
