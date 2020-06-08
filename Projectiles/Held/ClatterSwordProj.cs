using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
    public class ClatterSwordProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Clatter Sword");
        }

        public override void SetDefaults() {
            projectile.CloneDefaults(ProjectileID.Trident);

            aiType = ProjectileID.Trident;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(6) == 0)
                target.AddBuff(ModContent.BuffType<ClatterPierce>(), 180);
        }

    }
}
