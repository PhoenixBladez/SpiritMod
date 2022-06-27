using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace SpiritMod.NPCs.DarkfeatherMage.Projectiles
{
	public class DarkfeatherBoltRegular : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Darkfeather Bolt");

		public override void SetDefaults()
		{
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 140;
			Projectile.height = 4;
			Projectile.width = 4;
			Projectile.hide = true;
		}
		public override void AI()
		{
            Projectile.ai[0] += .1135f;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			int num623 = Dust.NewDust(Projectile.Center, 4, 4, DustID.ChlorophyteWeapon, 0f, 0f, 0, default, 1f);
            Main.dust[num623].shader = GameShaders.Armor.GetSecondaryShader(69, Main.LocalPlayer);
			Main.dust[num623].velocity = Projectile.velocity;
            Main.dust[num623].scale = MathHelper.Clamp(1.6f, .9f, 10 / Projectile.ai[0]);
            Main.dust[num623].noGravity = Projectile.scale > .5f;
		}
	}
}
