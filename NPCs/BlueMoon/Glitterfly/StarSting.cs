using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon.Glitterfly
{
	public class StarSting : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Star Sting");

		public override void SetDefaults()
		{
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.height = 20;
			Projectile.width = 12;
			AIType = ProjectileID.DeathLaser;
			Projectile.extraUpdates = 1;
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(Projectile, new StandardColorTrail(new Color(255, 214, 99)), new RoundCap(), new DefaultTrailPosition(), 80f, 450f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_1").Value, 0.01f, 1f, 1f));

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;

			for (int i = 0; i < 2; i++) {
				float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
				float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, DustID.VilePowder, 0f, 0f, 0, default, 1f);
				Main.dust[num].alpha = Projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.VilePowder, 0f, 0f, 100, default, 3f);
			Main.dust[num624].velocity *= 0f;
			Main.dust[num624].scale *= 0.3f;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}
