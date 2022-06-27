using Terraria;
using Terraria.GameContent;
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.FallingStar);
            AIType = ProjectileID.FallingStar;
            Projectile.timeLeft = 600;
        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.type = ProjectileID.FallingStar;
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(220, 220, 220, 150);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.25f, Projectile.height * 0.25f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                //Vector2 drawPos1 = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY - 4);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                //Main.spriteBatch.Draw(SpiritMod.Instance.GetTexture("Effects/Masks/Extra_A1"), drawPos1, null, new Color (0, 50, 155, (int)((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length)), 0f, drawOrigin, .6f, SpriteEffects.None, 0f);

            }
            return false;
        }
    }
}
