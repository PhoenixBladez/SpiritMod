using SpiritMod.Buffs;
using SpiritMod.Items.Weapon.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
    public class ApocalypseProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Apocalypse");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults() {
            projectile.width = 10;
            projectile.height = 16;

            projectile.aiStyle = 1;
            projectile.aiStyle = 113;

            projectile.ranged = true;
            projectile.friendly = true;

            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            aiType = ProjectileID.BoneJavelin;
        }

        public override void AI() {
            if(Main.rand.Next(5) == 0) {
                int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 61, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(ModContent.BuffType<FelBrand>(), 300);
        }

        public override void Kill(int timeLeft) {
            if(Main.rand.Next(0, 4) == 0)
                Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, ModContent.ItemType<Apocalypse>(), 1, false, 0, false, false);

            Main.PlaySound(SoundID.NPCKilled, (int)projectile.position.X, (int)projectile.position.Y, 6);
            for(int I = 0; I < 8; I++)
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 61, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
        }

    }
}
