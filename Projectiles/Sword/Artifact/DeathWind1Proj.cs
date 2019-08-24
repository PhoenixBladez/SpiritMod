using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Sword.Artifact
{
	public class DeathWind1Proj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death Wind");
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.penetrate = -1;
			projectile.timeLeft = 600;
			projectile.extraUpdates = 1;
			aiType = ProjectileID.WoodenBoomerang;
			projectile.tileCollide = false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.25f, projectile.height * 0.25f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void AI()
		{
			MyPlayer mp = Main.player[projectile.owner].GetModPlayer<MyPlayer>(mod);
			if (mp.DarkBough)
			{
				int dust1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 60);
				Main.dust[dust1].noGravity = true;
			}
			int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 110);
			Main.dust[dust2].noGravity = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(mod.BuffType("DeathWreathe"), 180);

			MyPlayer mp = Main.player[projectile.owner].GetModPlayer<MyPlayer>(mod);
			if (mp.DarkBough)
			{
				if (Main.rand.Next(10) == 0)
					Projectile.NewProjectile(target.Center.X, target.Center.Y, 0f, 0f, mod.ProjectileType("BoughSeed"), projectile.damage / 3 * 2, 4, projectile.owner);
			}
		}

	}
}
