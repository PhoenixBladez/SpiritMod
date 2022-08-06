using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class TeleportBolt : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Teleport Bolt");

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.WoodenArrowFriendly;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.damage = 0;
			Projectile.alpha = 255;
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(Projectile, new StandardColorTrail(new Color(122, 233, 255) * .6f), new RoundCap(), new SleepingStarTrailPosition(), 15f, 130f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_2").Value, 0.01f, 1f, 1f));

		public override bool PreAI()
		{
			Projectile.rotation += 0.1f;
			if (Main.rand.NextBool(3)) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center, 226);
				dust.velocity = Vector2.Zero;
				dust.noGravity = true;
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
			SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
			//Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1);
			for (int num424 = 0; num424 < 10; num424++) {
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Coralstone, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default, 0.75f);
			}
			Teleport(new Vector2(Projectile.position.X, Projectile.position.Y - 32), 2, 0);
		}
		public void Teleport(Vector2 newPos, int Style = 0, int extraInfo = 0)
		{
			Player player = Main.player[Projectile.owner];
			try {
				player.grappling[0] = -1;
				player.grapCount = 0;
				for (int j = 0; j < 1000; j++) {
					if (Main.projectile[j].active && Main.projectile[j].owner == player.whoAmI && Main.projectile[j].aiStyle == 7) {
						Main.projectile[j].Kill();
					}
				}
				int extraInfo2 = 0;
				if (Style == 4) {
					extraInfo2 = player.lastPortalColorIndex;
				}
				float num3 = MathHelper.Clamp(1f - player.teleportTime * 0.99f, 0.01f, 1f);
				float num2 = Vector2.Distance(player.position, newPos);
				player.position = newPos;
				player.fallStart = (int)(player.position.Y / 16f);
				if (player.whoAmI == Main.myPlayer) {
					bool flag = false;
					if (num2 < new Vector2((float)Main.screenWidth, (float)Main.screenHeight).Length() / 2f + 100f) {
						int time = 0;
						if (Style == 1) {
							time = 10;
						}
						Main.SetCameraLerp(0.1f, time);
						flag = true;
					}
					else {
						Main.BlackFadeIn = 255;
						Lighting.Clear();
						Main.screenLastPosition = Main.screenPosition;
						Main.screenPosition.X = player.position.X + (float)(player.width / 2) - (float)(Main.screenWidth / 2);
						Main.screenPosition.Y = player.position.Y + (float)(player.height / 2) - (float)(Main.screenHeight / 2);
						Main.instantBGTransitionCounter = 10;
					}
					if (num3 > 0.1f || !flag || Style != 0) {
						if (Main.mapTime < 5) {
							Main.mapTime = 5;
						}
						Main.maxQ = true;
						Main.renderNow = true;
					}
				}
				if (Style == 4) {
					player.lastPortalColorIndex = extraInfo;
					extraInfo2 = player.lastPortalColorIndex;
					player.portalPhysicsFlag = true;
					player.gravity = 0f;
				}
				for (int i = 0; i < 3; i++) {
					player.UpdateSocialShadow();
				}
				player.oldPosition = player.position + player.BlehOldPositionFixer;
				player.teleportTime = 1f;
				player.teleportStyle = Style;
			}
			catch {
			}
		}
	}
}
