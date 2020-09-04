using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Projectiles.Thrown.Artifact
{
	public class Miasma : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miasma");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 22;
			projectile.height = 22;
			projectile.aiStyle = -1;
			projectile.minion = true;
            projectile.alpha = 100;
			projectile.friendly = true;
			projectile.penetrate = 3;
            projectile.scale = .9f;
			projectile.timeLeft = 120;
		}

        public override void AI()
        {
            projectile.velocity *= .98f;
            projectile.alpha++;
            projectile.spriteDirection = projectile.direction;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 6)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame >= 4)
                    projectile.frame = 0;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(5) == 1)
                target.AddBuff(BuffID.Poisoned, 120);
        }
        public override void Kill(int timeLeft)
        {
            for (int num621 = 0; num621 < 2; num621++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height,
                    2, 0f, 0f, 100, default(Color), .7f);
                Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height, ModContent.DustType<Dusts.PoisonGas>(), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, new Color(), 4f)];
                dust.noGravity = true;
                dust.velocity.X = dust.velocity.X * 0.3f;
                dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;
            }
        }
    }
}
