using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class TargetCan : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Target Can");
		}
		bool shot = false;
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Shuriken);
			projectile.width = 25;
			projectile.damage = 0;
			projectile.height = 25;
            projectile.ranged = false;
        }
		
		public override void AI()
		{
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
			foreach (var proj in list)
			{
				if (proj.ranged && proj.active && !shot && proj.friendly && !proj.hostile && proj.width <= 6 && proj.height <= 6)
				{
					Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
					shot = true;
					projectile.damage = (int)MathHelper.Clamp(proj.damage * 4, 0, 220);
					projectile.velocity = proj.velocity * 2;
					proj.active = false;
					 CombatText.NewText(new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height), new Color(255, 155, 0, 100),
					"Bullseye!");
					projectile.ranged = true;
					projectile.penetrate = 2;
				}
			}
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