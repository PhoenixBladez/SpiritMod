using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Sword
{

	public class NailProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hollow Nail");
			Main.projFrames[base.projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.width = 56;
			projectile.height = 66;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 11;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 2) {
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			Vector2 angle = new Vector2(projectile.ai[0], projectile.ai[1]);
			projectile.rotation = angle.ToRotation();
			Player player = Main.player[projectile.owner];
			projectile.position = player.Center + angle - new Vector2(projectile.width / 2, projectile.height / 2);
			if (projectile.timeLeft == 2) {
				projectile.friendly = false;
			}
			if (projectile.timeLeft % 3 == 0)
			Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<HollowDust>());
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Vector2 angle = new Vector2(projectile.ai[0], projectile.ai[1]);
			angle *= 0.105f;
			Player player = Main.player[projectile.owner];
			if (angle.Y > 0 && player.velocity.Y != 0) {
				angle *= 2.5f;
				player.velocity.Y = -angle.Y;
			}
			base.OnHitNPC(target, damage, knockback, crit);
		}

	}
}