using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.NPCs.Snaptrapper
{
    public class SnaptrapperGas : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Poison Cloud");
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            projectile.timeLeft = 127;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.scale *= .8f;
            projectile.alpha = 60;
        }

        public override void AI()
        {
            projectile.velocity *= .98f;
            projectile.velocity.Y += .0125f;
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
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(2) == 1)
                target.AddBuff(BuffID.Poisoned, 300);
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