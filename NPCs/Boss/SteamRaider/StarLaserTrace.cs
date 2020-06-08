using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
    public class StarLaserTrace : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Star Laser");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.width = 2;
            projectile.height = 2;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.penetrate = 8;
            projectile.alpha = 255;
            projectile.timeLeft = 60;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 1;
        }


        public override bool PreAI()
        {
            int num = 5;
                int index2 = Dust.NewDust(projectile.position, 1, 1, 206, 0.0f, 0.0f, 0, new Color(), 1.3f);
                Main.dust[index2].position = projectile.Center - projectile.velocity / num;
                Main.dust[index2].velocity *= 0f;
                Main.dust[index2].noGravity = true;
                Main.dust[index2].noLight = true;
			return true;
		}

    }
}