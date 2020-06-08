using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Block;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class SpiritSand : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Spirit Sand");
        }

        public override void SetDefaults() {
            projectile.width = 12;
            projectile.height = 12;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 60000;
            projectile.tileCollide = true;
            projectile.aiStyle = 1;
            projectile.ignoreWater = true;
        }

        public override bool PreAI() {
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 103, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
            projectile.velocity.Y *= 3f;
            projectile.rotation += .2f;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            WorldGen.PlaceTile((int)(projectile.position.X / 16), (int)(projectile.position.Y / 16), ModContent.TileType<Spiritsand>());
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 103, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            int dust1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 103, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 103, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            int dust3 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 103, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            int dust4 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 103, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            projectile.Kill();
            return true;
        }
    }
}