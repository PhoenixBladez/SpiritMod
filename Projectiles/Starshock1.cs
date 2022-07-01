using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Starshock1 : ModProjectile, ITrailProjectile

    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starshock");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.alpha = 255;
			Projectile.timeLeft = 140;
			Projectile.penetrate = 2;
			Projectile.extraUpdates = 1;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(108, 215, 245), new Color(105, 213, 255)), new RoundCap(), new DefaultTrailPosition(), 8f, 150f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_2").Value, 0.01f, 1f, 1f));
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 255, 255) * .25f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 26f, 250f, new DefaultShader());
		}

		public override void AI()
		{
			float num1 = 5f;
			float num2 = 3f;
			float num3 = 20f;
			num1 = 6f;
			num2 = 3.5f;
			if (Projectile.timeLeft > 30 && Projectile.alpha > 0)
				Projectile.alpha -= 25;
			if (Projectile.timeLeft > 30 && Projectile.alpha < 128 && Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				Projectile.alpha = 128;
			if (Projectile.alpha < 0)
				Projectile.alpha = 0;

			if (++Projectile.frameCounter > 4) {
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 4)
					Projectile.frame = 0;
			}

			++Projectile.ai[1];
			double num5 = (double)Projectile.ai[1] / 180.0;


			if (Main.player[Projectile.owner].active && !Main.player[Projectile.owner].dead) {
				if (Projectile.Distance(Main.player[Projectile.owner].Center) <= num3)
					return;
				Vector2 unitY = Projectile.DirectionTo(Main.player[Projectile.owner].Center);
				if (unitY.HasNaNs())
					unitY = Vector2.UnitY;
				Projectile.velocity = (Projectile.velocity * (num1 - 1f) + unitY * num2) / num1;
			}
			else {
				if (Projectile.timeLeft > 30)
					Projectile.timeLeft = 30;
				if (Projectile.ai[0] == -1f)
					return;
				Projectile.ai[0] = -1f;
				Projectile.netUpdate = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.25f, Projectile.height * 0.25f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				//Vector2 drawPos1 = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY - 4);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				//Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Effects/Masks/Extra_A1"), drawPos1, null, new Color (0, 50, 155, (int)((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length)), 0f, drawOrigin, .6f, SpriteEffects.None, 0f);

			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void Kill(int timeLeft)
		{
			int z = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Wrath>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            Main.projectile[z].DamageType = DamageClass.Magic;
			SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.Center);
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width = 5;
			Projectile.height = 5;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
			for (int num621 = 0; num621 < 10; num621++) {
				int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, default, 1f);
				Main.dust[num622].velocity *= 1f;
				Main.dust[num622].noGravity = true;

			}
			for (int num623 = 0; num623 < 15; num623++) {
				int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, default, .31f);
				Main.dust[num624].velocity *= .5f;
			}
		}
	}
}