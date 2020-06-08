
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class Dazzle : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Dazzle");
        }

        public override void SetDefaults() {
            projectile.width = 10;       //projectile width
            projectile.height = 20;  //projectile height
            projectile.friendly = true;      //make that the projectile will not damage you
            projectile.melee = true;         // 
            projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
            projectile.penetrate = 1;      //how many npc will penetrate
            projectile.timeLeft = 30;   //how many time projectile projectile has before disepire
            projectile.light = 0.75f;    // projectile light
            projectile.extraUpdates = 1;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
            projectile.aiStyle = -1;
        }

        public override void AI() {
            for(int npcFinder = 0; npcFinder < 200; ++npcFinder) {
                if(!Main.npc[npcFinder].boss && !Main.npc[npcFinder].townNPC) {
                    Main.npc[npcFinder].AddBuff(BuffID.Confused, 240);
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            return false;
        }

        public override void Kill(int timeLeft) {
            for(int num621 = 0; num621 < 40; num621++) {
                int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.GoldCoin, 0f, 0f, 100, default(Color), 1f);
                Main.dust[num622].velocity *= .5f;
                {
                    Main.dust[num622].scale = 0.5f;
                }
            }
        }

    }
}
