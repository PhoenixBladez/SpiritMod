using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Projectiles.Returning
{
	public class ReachBoomerang : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Briarheart Boomerang");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.magic = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 700;
		}
		 private Texture2D GlowingTrail => GetTexture("SpiritMod/Projectiles/Returning/ReachBoomerang_Trail");

		public override void AI()
		{
			projectile.rotation += 0.1f;
			{
				int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Plantera_Green, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust2].velocity *= 0f;
				Main.dust[dust2].scale = .62f;
				Main.dust[dust2].noGravity = true;
			}
		}
		 public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
				 Color color = projectile.GetAlpha(Color.White) * (((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * 0.5f);
                float scale = projectile.scale * (float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length;

                spriteBatch.Draw(GlowingTrail,
                projectile.oldPos[k] + drawOrigin - Main.screenPosition,
                new Rectangle(0, (Main.projectileTexture[projectile.type].Height / 2) * projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 2),
                color,
                projectile.rotation,
                new Vector2(Main.projectileTexture[projectile.type].Width / 2, Main.projectileTexture[projectile.type].Height / 4),
                scale, default, default);
			}
			return true;
		}
	}
}
