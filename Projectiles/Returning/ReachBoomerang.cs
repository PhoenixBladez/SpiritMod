using Terraria;
using Terraria.GameContent;
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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = 3;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 700;
		}
		 private Texture2D GlowingTrail => GetTexture("SpiritMod/Projectiles/Returning/ReachBoomerang_Trail");

		public override void AI()
		{
			Projectile.rotation += 0.1f;
			{
				int dust2 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Plantera_Green, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Main.dust[dust2].velocity *= 0f;
				Main.dust[dust2].scale = .62f;
				Main.dust[dust2].noGravity = true;
			}
		}
		public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
				Color color = Projectile.GetAlpha(Color.White) * (((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f);
                float scale = Projectile.scale * (float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;

				Main.spriteBatch.Draw(GlowingTrail,
                Projectile.oldPos[k] + drawOrigin - Main.screenPosition,
                new Rectangle(0, (TextureAssets.Projectile[Projectile.type].Value.Height / 2) * Projectile.frame, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height / 2),
                color,
                Projectile.rotation,
                new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width / 2, TextureAssets.Projectile[Projectile.type].Value.Height / 4),
                scale, default, default);
			}
			return true;
		}
	}
}
