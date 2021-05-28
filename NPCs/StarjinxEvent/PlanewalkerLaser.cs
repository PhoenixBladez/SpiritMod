using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
    public class PlanewalkerLaser : ModProjectile
    {
        public int chosenDust = 1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nova Ray");
        }
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.hide = true;
            projectile.timeLeft = 15;
            projectile.extraUpdates = 2;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.scale = 1f;
            projectile.penetrate = 30;
            projectile.ignoreWater = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int num2 = Main.rand.Next(20, 40);
            for (int index1 = 0; index1 < num2; ++index1)
            {
                int index2 = Dust.NewDust(projectile.Center, 0, 0, chosenDust, 0.0f, 0.0f, 100, new Color(), 1.2f);
                Main.dust[index2].velocity *= 1.2f;
                --Main.dust[index2].velocity.Y;
                Main.dust[index2].velocity += projectile.velocity;
                Main.dust[index2].noGravity = true;
            }
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (chosenDust == 1)
            {
                Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 12, 1f, 0f);
                chosenDust = Main.rand.Next(2) == 0 ? 159 : 164;
                CircleOfDust();
            }

            for (int index1 = 0; index1 < 5; ++index1)
            {
                Vector2 Position = projectile.position - projectile.velocity * ((float)index1 * 0.3334f);
                projectile.alpha = (int)byte.MaxValue;
                Dust dust = Dust.NewDustPerfect(Position, chosenDust);
                dust.scale = (float)Main.rand.Next(30, 80) * 0.013f;
                dust.velocity *= 0.2f;
                dust.noGravity = true;
                dust.noLight = true;
            }
            for (int index1 = 0; index1 < 10; ++index1)
            {
                float x = projectile.position.X - projectile.velocity.X / 10f * (float)index1;
                float y = projectile.position.Y - projectile.velocity.Y / 10f * (float)index1;
                Dust dust = Dust.NewDustPerfect(new Vector2(x, y), chosenDust);
                dust.alpha = projectile.alpha;
                dust.position.X = x;
                dust.position.Y = y;
                dust.velocity *= 0.0f;
                dust.noGravity = true;
                dust.noLight = true;
            }
        }
        internal void CircleOfDust()
        {
            Player player = Main.player[projectile.owner];
            for (int i = 0; i < 36; i++)
            {
                Vector2 direction = Main.rand.NextFloat(6.28f).ToRotationVector2();
                Dust dust = Dust.NewDustPerfect(projectile.position, chosenDust);
                dust.velocity = direction * 3 * Main.rand.NextFloat();
                dust.noGravity = true;
                dust.scale = 2f;
                dust.fadeIn = Main.rand.NextFloat(0.3f, 1f) * 2f;
                Dust dust2 = Dust.CloneDust(dust);
                dust2.scale /= 2f;
                dust2.fadeIn /= 2f;
            }
        }
        public override void Kill(int timeLeft)
        {
            CircleOfDust();
        }
    }
}