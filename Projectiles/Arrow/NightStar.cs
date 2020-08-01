using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SpiritMod.Projectiles.Arrow
{
	public class NightStar : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Night Star");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.FallingStar);
            aiType = ProjectileID.FallingStar;
            projectile.timeLeft = 600;
        }

        public override bool PreKill(int timeLeft)
        {
            projectile.type = ProjectileID.FallingStar;
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(220, 220, 220, 150);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.25f, projectile.height * 0.25f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                //Vector2 drawPos1 = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY - 4);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
                //Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_A1"), drawPos1, null, new Color (0, 50, 155, (int)((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length)), 0f, drawOrigin, .6f, SpriteEffects.None, 0f);

            }
            return false;
        }
    }
}
