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
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = 127;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale *= .8f;
            Projectile.alpha = 60;
        }

        public override void AI()
        {
            Projectile.velocity *= .98f;
            Projectile.velocity.Y += .0125f;
            Projectile.alpha++;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= 4)
                    Projectile.frame = 0;
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(BuffID.Poisoned, 300);
        }
        public override void Kill(int timeLeft)
        {
            for (int num621 = 0; num621 < 2; num621++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, 0f, 0f, 100, default, .7f);
                Dust dust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width, Projectile.height, ModContent.DustType<Dusts.PoisonGas>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, new Color(), 4f)];
                dust.noGravity = true;
                dust.velocity.X = dust.velocity.X * 0.3f;
                dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;
            }
        }
    }
}