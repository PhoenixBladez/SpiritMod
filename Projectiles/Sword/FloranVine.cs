using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Sword
{
    public class FloranVine : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Floran Vine");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.width = 4;
            projectile.height = 4;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.damage = 1;
            projectile.penetrate = 2;
            projectile.alpha = 255;
            projectile.timeLeft = 1;
            projectile.tileCollide = true;
            projectile.extraUpdates = 3;
        }

        int counter = 0;
        private void Trail(Vector2 from, Vector2 to) {
            float distance = Vector2.Distance(from, to);
            float step = 1 / distance;
            for(float w = 0; w < distance; w += 4) {
                Dust.NewDustPerfect(Vector2.Lerp(from, to, w * step), 39, Vector2.Zero).noGravity = true;
            }
        }
        public override bool PreAI() {
            Trail(projectile.position, projectile.position + projectile.velocity);
            switch(Main.rand.Next(4)) {
                case 0:
                    projectile.position.X += 3;
                    projectile.position.Y -= 3;
                    break;
                case 1:
                    projectile.position.Y += 3;
                    projectile.position.X -= 3;
                    break;
                case 2:
                    projectile.position.Y += 3;
                    projectile.position.X += 3;
                    break;
                case 3:
                    projectile.position.Y -= 3;
                    projectile.position.X -= 3;
                    break;
            }
            return true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(5) == 0)
                target.AddBuff(ModContent.BuffType<VineTrap>(), 180);
        }

    }
}