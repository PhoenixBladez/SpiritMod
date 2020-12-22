using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class OrionOrb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orion Orb");
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Shuriken);
			projectile.width = 40;
			projectile.damage = 0;
			projectile.height = 40;
			projectile.ranged = false;
		}
		public override void AI()
		{
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
			foreach (var proj in list) {
				if (proj.ranged && proj.active && proj.friendly && !proj.hostile && (proj.width <= 6 || proj.height <= 6)) {
					Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 27);
					Player player = Main.player[projectile.owner];
					player.AddBuff(ModContent.BuffType<OverchargedOrion>(), 120);
					projectile.active = false;
					Main.PlaySound(SoundID.Item93, player.position);
					CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 0, 0), new Color(0, 155, 255, 100),
				   "Overcharged!");
				   player.GetModPlayer<MyPlayer>().Shake += 16;
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 27);
			for (int num424 = 0; num424 < 15; num424++) {
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 13, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 0, default(Color), 0.75f);
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