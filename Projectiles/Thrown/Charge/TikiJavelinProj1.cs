using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Thrown.Charge
{
	public class TikiJavelinProj1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiki Javelin Tracer");
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 39;
			Projectile.aiStyle = 1;
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 16;
			Projectile.light = 0;
			Projectile.ignoreWater = true;
			AIType = ProjectileID.ThrowingKnife;
		}
		public override bool PreAI()
		{
			if (Projectile.owner != Main.myPlayer) return true;
			int num = 5;
			int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.Torch, 0.0f, 0.0f, 0, new Color(), 1.3f);
			Main.dust[index2].position = Projectile.Center - Projectile.velocity / num;
			Main.dust[index2].velocity *= 0f;
			Main.dust[index2].noGravity = true;
			Main.dust[index2].noLight = true;
			return true;
		}
	}
}
