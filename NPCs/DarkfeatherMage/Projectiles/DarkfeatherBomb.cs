using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Graphics.Shaders;
using SpiritMod.Projectiles;

namespace SpiritMod.NPCs.DarkfeatherMage.Projectiles
{
	public class DarkfeatherBomb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Darkfeather Bomb");
        }

		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
            Projectile.hostile = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 90;
		}

		public override void AI()
		{
            Projectile.velocity.Y += .185f;
            Vector2 currentSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
            Projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / 40));
            Projectile.ai[0] += .1135f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            int num623 = Dust.NewDust(Projectile.Center, 4, 4, DustID.ChlorophyteWeapon, 0f, 0f, 0, default, 1f);
            Main.dust[num623].shader = GameShaders.Armor.GetSecondaryShader(67, Main.LocalPlayer);
            if (Projectile.scale > .5f)
                Main.dust[num623].noGravity = true;
            else
                Main.dust[num623].noGravity = false;
            Main.dust[num623].velocity = Projectile.velocity;
            Main.dust[num623].scale = MathHelper.Clamp(1.6f, .9f, 10 / Projectile.ai[0]);
        }
        public override void Kill(int timeLeft)
        {
            int num = 0;
			if (Projectile.friendly)
                num = 120;
			else
                num = 80;
            ProjectileExtras.Explode(Projectile.whoAmI, num, num,
            delegate
            {
                if (Projectile.friendly)
                    SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.Center);
                else
                    SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);

                for (int k = 0; k < 35; k++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, 159, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, Main.rand.NextFloat(.8f, 1.2f));
                    d.noGravity = false;
                    d.shader = GameShaders.Armor.GetSecondaryShader(67, Main.LocalPlayer);
                }
            });
        }
	}
}
