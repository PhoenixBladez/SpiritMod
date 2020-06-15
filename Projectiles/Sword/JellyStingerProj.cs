using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Projectiles.Sword;

namespace SpiritMod.Projectiles.Sword
{
    public class JellyStingerProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Jelly Stinger");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.thrown = true;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.timeLeft = 35;
            projectile.tileCollide = false;
        }
        public override bool PreAI() {
            Player player = Main.player[projectile.owner];
             projectile.velocity = Vector2.Zero;
            projectile.position = player.Center;
            player.heldProj = projectile.whoAmI;
            //	player.itemRotation = 0;
            return true;
        }
    }
}
