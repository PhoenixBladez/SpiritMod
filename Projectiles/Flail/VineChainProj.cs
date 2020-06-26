
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
    public class VineChainProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Vine Chain");
        }

        public override void SetDefaults() {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 900;
            projectile.melee = true;
        }
      //  bool comingHome = false;
      int hookednpc = 0;
      bool hooked = false;
      float returnSpeed = 7;
        public override bool PreAI() {
             if (projectile.Hitbox.Intersects(Main.player[projectile.owner].Hitbox) && projectile.timeLeft < 870)
            {
                projectile.active = false;
            }
            if (projectile.timeLeft < 859)
            {
                Vector2 direction9 = Main.player[projectile.owner].Center - projectile.position;
                direction9.Normalize();
                projectile.velocity = direction9 * returnSpeed;
                returnSpeed+=0.2f;
                projectile.rotation = projectile.velocity.ToRotation() - 1.57f;
            }
            else
            {
                 projectile.velocity -= new Vector2(projectile.ai[0], projectile.ai[1]) / 40f;
                 projectile.rotation = new Vector2(projectile.ai[0], projectile.ai[1]).ToRotation() +1.57f;
            }
             Player player = Main.player[projectile.owner];
             player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            if (!player.channel || hooked)
            {
                projectile.timeLeft = 858;
                projectile.friendly = false;
            }
            NPC npc = Main.npc[hookednpc];
            if (hooked && npc.active && player.channel)
            {
                int distance = (int)Math.Sqrt((npc.Center.X - player.Center.X) * (npc.Center.X - player.Center.X) + (npc.Center.Y - player.Center.Y) * (npc.Center.Y - player.Center.Y));
                if (distance > 100)
                {
                    npc.velocity = projectile.velocity;
                }
                else
                {
                    hooked = false;
                }
            }
             return false;

        }
        //projectile.ai[0]: X speed initial
        //projectile.ai[1]: y speed initial

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            ProjectileExtras.DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter,
            "SpiritMod/Projectiles/Flail/VineChain_Chain");
            ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(!target.boss && target.knockBackResist != 0)
            {
                hooked = true;
                hookednpc = target.whoAmI;
                 target.position = projectile.position - new Vector2((target.width / 2), (target.height / 2));;
               //  target.velocity = projectile.velocity;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.timeLeft = 800;
            projectile.friendly = false;
            return false;
        }
    }
}
