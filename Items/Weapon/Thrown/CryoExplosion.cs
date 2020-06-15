using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using SpiritMod.Projectiles.Thrown;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
    public class CryoExplosion : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Cryolite Bomb");
             Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults() {
            projectile.width = 80;
            projectile.height = 80;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 150;
            projectile.alpha = 0;
         //   aiType = ProjectileID.ThrowingKnife;
            projectile.tileCollide = false;
        }
       float counter = 3f;
        public override void Kill(int timeLeft) {

        }
        public override bool PreAI()
        {
            if (counter > 0)
            {
             counter-=0.25f;
            }
            projectile.frame = (int)counter;
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(4) == 0)
                target.AddBuff(ModContent.BuffType<CryoCrush>(), 240);
        }
    }
}