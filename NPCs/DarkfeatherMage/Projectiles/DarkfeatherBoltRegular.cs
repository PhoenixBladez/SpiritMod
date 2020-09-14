using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace SpiritMod.NPCs.DarkfeatherMage.Projectiles
{
	public class DarkfeatherBoltRegular : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Darkfeather Bolt");
		}

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
			int num623 = Dust.NewDust(projectile.Center, 4, 4,
				157, 0f, 0f, 0, default(Color), 1f);
            Main.dust[num623].shader = GameShaders.Armor.GetSecondaryShader(69, Main.LocalPlayer);
            if (projectile.scale > .5f)
            {
                Main.dust[num623].noGravity = true;
            }
			else
            {
                Main.dust[num623].noGravity = false;
            }
			Main.dust[num623].velocity = projectile.velocity;
            Main.dust[num623].scale = MathHelper.Clamp(1.6f, .9f, 10 / projectile.ai[0]);
        }
	}
}
