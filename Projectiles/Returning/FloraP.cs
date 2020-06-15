using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
namespace SpiritMod.Projectiles.Returning
{
    public class FloraP : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Floran Rollar");
        }

        public override void SetDefaults() {
            projectile.width = 40;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.magic = false;
            projectile.penetrate = 6;
            projectile.timeLeft = 300;
            projectile.extraUpdates = 1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(5) == 0)
                target.AddBuff(ModContent.BuffType<VineTrap>(), 180);
           // projectile.velocity.X = 0;
        }
         public override void Kill(int timeLeft) {
            for(int i = 0; i < 40; i++) {
                int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 39, (float)(Main.rand.Next(8) - 4), (float)(Main.rand.Next(8) - 4), 133);
            }
        }
        int direction = 0; //0 is left, 1 is right
        float rotation = 2;
        int jumpCounter = 6;
        bool stopped = false;
         public override bool PreAI() {
             jumpCounter++;
            rotation *= 1.005F;
            projectile.velocity.Y += 0.4F;
            projectile.velocity.X *= 1.005F;
            projectile.velocity.X = MathHelper.Clamp(projectile.velocity.X, -10, 10);
            if (projectile.velocity.X > 0)
            {
                direction = 1;
            }
            if (projectile.velocity.X < 0)
            {
                direction = 0;
            }
            if (direction == 0)
            {
                projectile.rotation -= rotation / 25;
            }
            else
            {
                projectile.rotation += rotation / 25;
            }
            return false;
        }
         public override bool OnTileCollide(Vector2 oldVelocity) {
             if (oldVelocity.X !=  projectile.velocity.X)
             {
                 if (jumpCounter > 5 && !stopped)
                 {
                    jumpCounter = 0;
                    projectile.position.Y -= 20;
                    projectile.velocity.X = oldVelocity.X;
                 }
                 else if (!stopped)
                 {
                   //   projectile.position.Y += 12;
                      stopped = true;
                      projectile.velocity.X = 0;
                 }
             }
            return false;
        }
          public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough) {
            fallThrough = false;
            return true;
        }

    }
}
