using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
    public class HuskstalkSpearProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Huskstalk Spear");
        }

        public override void SetDefaults() {
            projectile.CloneDefaults(ProjectileID.Trident);
            aiType = ProjectileID.Trident;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(2) == 0)
                target.AddBuff(ModContent.BuffType<WitheringLeaf>(), 180);
        }

    }
}
