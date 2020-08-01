
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Flail
{
	public class MagnetFlailProj : ModProjectile
	{
		bool readytostick = true;
		bool stuck = false;
		int timeStuck = 150;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnet Flail");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
		}

		public override bool PreAI()
		{
			if (!stuck) {
				ProjectileExtras.FlailAI(projectile.whoAmI);
			}
			else {
				Player player = Main.player[projectile.owner];
					int dust = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 226);
					Main.dust[dust].velocity *= -1f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale *= .6f;
					Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
					vector2_1.Normalize();
					Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
					Main.dust[dust].velocity = vector2_2;
					vector2_2.Normalize();
					Vector2 vector2_3 = vector2_2 * 34f;
					Main.dust[dust].position = projectile.Center - vector2_3;
				timeStuck--;
				if (timeStuck < 0) {
					stuck = false;
				}
				player.itemTime = 5;
				player.itemAnimation = 5;
				projectile.velocity.X = 0;
				projectile.velocity.Y = 0;
			}
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!readytostick) {
				return ProjectileExtras.FlailTileCollide(projectile.whoAmI, oldVelocity);
			}
			else {
				Main.PlaySound(SoundID.Item93, projectile.position);
				readytostick = false;
				stuck = true;
				if (oldVelocity.X != projectile.velocity.X) //if its an X axis collision
				{
					if (projectile.velocity.X > 0) {
						projectile.rotation = 1.57f;
					}
					else {
						projectile.rotation = 4.71f;
					}
				}
				if (oldVelocity.Y != projectile.velocity.Y) //if its a Y axis collision
				{
					if (projectile.velocity.Y > 0) {
						projectile.rotation = 3.14f;
					}
					else {
						projectile.rotation = 0f;
					}
				}
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (stuck) {
				ProjectileExtras.DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter,
				"SpiritMod/Projectiles/Flail/MagnetFlail_ElectricChain", true, projectile.damage / 2);
				if (timeStuck % 3 == 0)
				{
					DustHelper.DrawElectricity(projectile.Center, Main.player[projectile.owner].MountedCenter, 226, 0.3f);
				}
			}
			else {
				ProjectileExtras.DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter,
				"SpiritMod/Projectiles/Flail/MagnetFlail_Chain");
			}
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}
	}
}
