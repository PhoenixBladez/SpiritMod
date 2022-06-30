using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Projectiles.Magic
{
	public class HallowedStaffProj : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Mageblade");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;       //projectile width
			Projectile.height = 20;  //projectile height
			Projectile.friendly = true;      //make that the projectile will not damage you
			Projectile.DamageType = DamageClass.Magic;         // 
			Projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
			Projectile.penetrate = 2;      //how many npc will penetrate
			Projectile.timeLeft = 390;   //how many time projectile projectile has before disepire // projectile light
			Projectile.extraUpdates = 1;
			Projectile.ignoreWater = true;
			Projectile.aiStyle = -1;
		}

		int timer;
		int colortimer;

		public void DoTrailCreation(TrailManager tManager)
		{
			switch (Main.rand.Next(3)) {
				case 0:
					tManager.CreateTrail(Projectile, new StandardColorTrail(new Color(115, 220, 255)), new RoundCap(), new SleepingStarTrailPosition(), 8f, 100f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_2").Value, 0.01f, 1f, 1f));
					break;
				case 1:
					tManager.CreateTrail(Projectile, new StandardColorTrail(new Color(255, 231, 145)), new RoundCap(), new SleepingStarTrailPosition(), 8f, 100f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_2").Value, 0.01f, 1f, 1f));
					break;
				case 2:
					tManager.CreateTrail(Projectile, new StandardColorTrail(new Color(255, 128, 244)), new RoundCap(), new SleepingStarTrailPosition(), 8f, 100f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_2").Value, 0.01f, 1f, 1f));
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
			Projectile.scale = num395 + 0.55f;
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			Lighting.AddLight((int)(Projectile.position.X / 16f), (int)(Projectile.position.Y / 16f), 0.396f / 2, 0.370588235f / 2, 0.364705882f / 2);

			int target = -1;

			for (int i = 0; i < Main.maxNPCs; i++) {
				if (Main.npc[i].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1)) {
					float num23 = Main.npc[i].position.X + (Main.npc[i].width / 2);
					float num24 = Main.npc[i].position.Y + (Main.npc[i].height / 2);
					float num25 = Math.Abs(Projectile.Center.X - num23) + Math.Abs(Projectile.Center.Y - num24);
					if (num25 < 500f) {
						target = i;
						break;
					}
				}
			}

			if (target != -1) {
				const float Speed = 12.5f;
				
				Vector2 direction5 = Vector2.Normalize(Main.npc[target].Center - Projectile.Center);
				Projectile.rotation = Projectile.DirectionTo(Main.npc[target].Center).ToRotation() + 1.57f;

				if (Main.rand.Next(16) == 0) {
					Projectile.velocity = direction5 * Speed;
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
					if (num416 > 8) {
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
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(60 + colortimer, 60 + colortimer, 60 + colortimer, 100);
	}
}
