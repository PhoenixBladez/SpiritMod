using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Projectiles.DonatorItems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Projectiles.Magic
{
	public class StarfallProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Falling Star");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(82);
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 28;
			projectile.light = 0.5f;
			projectile.height = 28;
			projectile.friendly = true;
			projectile.damage = 10;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(ModContent.BuffType<StarFracture>(), 280);
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + (float)(Math.PI / 2);
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Firework_Yellow);
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Wrath>(), projectile.damage / 3 * 2, projectile.knockBack, projectile.owner, 0f, 0f);

			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
			for (int num623 = 0; num623 < 70; num623++) {
				int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Firework_Yellow, 0f, 0f, 100, default, 1f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 1.5f;
				num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Firework_Yellow, 0f, 0f, 100, default, 1f);
				Main.dust[num624].velocity *= 2f;
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