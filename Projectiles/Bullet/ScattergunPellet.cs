using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class ScattergunPellet : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scattergun Pellet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 120;
			Projectile.height = 6;
			Projectile.width = 6;
			AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 1;
		}

		int timer = 0;
        bool chosen;
        int dustVer;
        int dustType;
		public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Vector2 mouse = Main.MouseWorld;
			if (!chosen)
            {
                dustVer = Main.rand.Next(0, 2);
                chosen = true;
            }
			if (dustVer == 0)
            {
                dustType = DustID.Electric;
            }
			else
            {
                dustType = 272;
            }
			timer++;
			if (timer == 48)
            {
                Projectile.velocity.Y *= -1;
				if (dustVer == 0)
                {
                    dustVer = 1;
                }
                else
                {
                    dustVer = 0;
                }
            }
            for (int i = 0; i < 3; i++) {
                float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
                float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
                int num = Dust.NewDust(new Vector2(x, y), 2, 2, dustType);
				Main.dust[num].alpha = Projectile.alpha;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
                Main.dust[num].fadeIn = 0.4684f;
                Main.dust[num].scale *= .1235f;
			}
            if (Main.rand.NextBool(3))
            {
                for (int i = 0; i < 1; i++)
                {
                    float x1 = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
                    float y1 = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
                    int num1 = Dust.NewDust(new Vector2(x1, y1), 2, 2, dustType);
                    Main.dust[num1].alpha = Projectile.alpha;
                    Main.dust[num1].velocity = Projectile.velocity;
                    Main.dust[num1].fadeIn = 0.4684f;
                    Main.dust[num1].noGravity = true;
                    Main.dust[num1].scale *= .1235f;
                }
            }
        }
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(240, 199, 255) * 0.65f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length); ;
				if (dustVer == 0)
					color = new Color(179, 237, 255) * 0.65f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

				float scale = Projectile.scale;
				Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Projectiles/Bullet/ScattergunPellet").Value;
				spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);
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
            return false;
        }
        public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.position);
		}
	}
}
