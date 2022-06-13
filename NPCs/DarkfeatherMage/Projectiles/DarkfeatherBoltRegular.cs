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
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 140;
			projectile.height = 4;
			projectile.width = 4;
			projectile.hide = true;
		}
		public override void AI()
		{
            projectile.ai[0] += .1135f;
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			int num623 = Dust.NewDust(projectile.Center, 4, 4, DustID.ChlorophyteWeapon, 0f, 0f, 0, default, 1f);
            Main.dust[num623].shader = GameShaders.Armor.GetSecondaryShader(69, Main.LocalPlayer);
			Main.dust[num623].velocity = projectile.velocity;
            Main.dust[num623].scale = MathHelper.Clamp(1.6f, .9f, 10 / projectile.ai[0]);
            Main.dust[num623].noGravity = projectile.scale > .5f;
		}
	}
}
