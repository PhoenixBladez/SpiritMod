using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Projectiles.Magic
{
	public class TrueHallowedStaffProj : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Mageblade");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;       //projectile width
			Projectile.height = 20;  //projectile height
			Projectile.friendly = true;      //make that the projectile will not damage you
			Projectile.DamageType = DamageClass.Magic;         // 
			Projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
			Projectile.penetrate = 3;      //how many npc will penetrate
			Projectile.timeLeft = 390;   //how many time projectile projectile has before disepire // projectile light
			Projectile.extraUpdates = 1;
			Projectile.ignoreWater = true;
			Projectile.aiStyle = -1;
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(Projectile, new RainbowTrail(5f, 0.002f, 1f, .75f), new RoundCap(), new SleepingStarTrailPosition(), 150f, 130f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_1").Value, 0.01f, 1f, 1f));

		int timer;
		int colortimer;
		public override void AI()
		{

			timer++;
			if (timer <= 50) {
				colortimer++;
			}
			if (timer > 50) {
				colortimer--;
			}
			if (timer >= 100) {
				timer = 0;
			}
			float num395 = Main.mouseTextColor / 155f - 0.35f;
			num395 *= 0.34f;
			Projectile.scale = num395 + 0.55f;
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			Lighting.AddLight((int)(Projectile.position.X / 16f), (int)(Projectile.position.Y / 16f), 0.396f / 2, 0.370588235f / 2, 0.364705882f / 2);
			bool flag25 = false;
			int jim = 1;
			for (int index1 = 0; index1 < 200; index1++) {
				if (Main.npc[index1].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1)) {
					float num23 = Main.npc[index1].position.X + (float)(Main.npc[index1].width / 2);
					float num24 = Main.npc[index1].position.Y + (float)(Main.npc[index1].height / 2);
					float num25 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num23) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num24);
					if (num25 < 500f) {
						flag25 = true;
						jim = index1;
					}

				}
			}
			if (flag25) {
				float num1 = 12.5f;
				Vector2 vector2 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num2 = Main.npc[jim].Center.X - vector2.X;
				float num3 = Main.npc[jim].Center.Y - vector2.Y;
				Vector2 direction5 = Main.npc[jim].Center - Projectile.Center;
				direction5.Normalize();
				Projectile.rotation = Projectile.DirectionTo(Main.npc[jim].Center).ToRotation() + 1.57f;
				float num4 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
				float num5 = num1 / num4;
				float num6 = num2 * num5;
				float num7 = num3 * num5;
				int num8 = 10;
				if (Main.rand.Next(16) == 0) {
					Projectile.velocity.X = (Projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
					Projectile.velocity.Y = (Projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
				}
			}

			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 7200f) {
				Projectile.alpha += 5;
				if (Projectile.alpha > 255) {
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 10f) {
				Projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = Projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == Projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
						num416++;
						if (Main.projectile[num420].ai[1] > num418) {
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
					if (num416 > 16) {
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						return;
					}
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(60 + colortimer, 60 + colortimer, 60 + colortimer, 100);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(target.position, target.width, target.height, DustID.Electric, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(77, Main.LocalPlayer);
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .25f;
				if (Main.dust[num].position != target.Center)
					Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 6f;
			}
		}
	}
}
