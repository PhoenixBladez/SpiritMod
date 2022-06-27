
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace SpiritMod.Projectiles.Summon
{
	class PinkJellyfishBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electric Bolt");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.minion = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 240;
			Projectile.height = 4;
			Projectile.width = 4;
			Projectile.alpha = 255;
			AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 3;
		}

        public override void AI()
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 position = Projectile.Center;
                Dust dust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Electric, 0f, 0f, 0, new Color(255, 255, 255), 0.464947368f)];
                dust.noLight = true;
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
				dust.shader = GameShaders.Armor.GetSecondaryShader(93, Main.LocalPlayer);
            }
			if (Main.rand.NextBool(3))
            {
                Vector2 position = Projectile.Center;
                Dust dust = Main.dust[Dust.NewDust(position, (int)Projectile.velocity.X, (int)Projectile.velocity.Y, DustID.Electric, 0f, 0f, 0, new Color(255, 255, 255), 0.464947368f)];
                dust.noLight = true;
                dust.noGravity = true;
                dust.velocity *= .6f;
				dust.shader = GameShaders.Armor.GetSecondaryShader(93, Main.LocalPlayer);
            }
        }

		public override void Kill(int timeLeft) => SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 3);

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 10; i++)
            {
                int num = Dust.NewDust(target.position, target.width, target.height, DustID.Electric, 0f, -2f, 0, default, 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(93, Main.LocalPlayer);
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].scale *= .25f;
                if (Main.dust[num].position != target.Center)
                    Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 3f;
            }
        }
    }
}
