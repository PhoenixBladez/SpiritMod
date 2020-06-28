using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class TargetShard : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Target Bottle");
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Shuriken);
			projectile.width = 6;
			projectile.height = 6;
			//projectile.aiStyle = 8;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.light = 0;
			//		aiType = ProjectileID.ThrowingKnife;
		}
		//public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		//{
		//    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
		//    for (int k = 0; k < projectile.oldPos.Length; k++)
		//    {
		//        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
		//        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
		//        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
		//    }
		//    return true;
		//}

	}
}