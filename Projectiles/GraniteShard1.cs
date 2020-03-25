using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Projectiles
{
	public class GraniteShard1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Shard");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 11;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.penetrate = 5;
			projectile.timeLeft = 600;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;
			aiType = ProjectileID.CrystalShard;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return false;
		}
		public override void AI()
		{

            for (int index1 = 0; index1 < 5; ++index1)
            {
                float num1 = projectile.velocity.X * 0.2f * (float)index1;
                float num2 = projectile.velocity.Y * -0.200000002980232f * index1;
                int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0.0f, 0.0f, 100, new Color(), 1.3f);
                Main.dust[index2].noGravity = true;
                Main.dust[index2].velocity *= 0.0f;
                Main.dust[index2].scale = .25f;
                Main.dust[index2].position.X -= num1;
                Main.dust[index2].position.Y -= num2;
            }
        }

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				int index2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
                Main.dust[index2].noGravity = true;
            }
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
		}
	}
}