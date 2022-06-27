using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.Spirit
{
	public class SpiritRainHostile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Arrow");
		}

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 800;
			Projectile.aiStyle = 1;
			Projectile.alpha = 255;
			Projectile.hide = true;
		}

		public override bool PreAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;

			for (int i = 0; i < 29; i++) {
				float x = Projectile.Center.X - Projectile.velocity.X / 2f * (float)i;
				float y = Projectile.Center.Y - Projectile.velocity.Y / 2f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, DustID.Flare_Blue, 0f, 0f, 0, default, 1f);
				Main.dust[num].alpha = Projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}
			return true;
		}

	}
}