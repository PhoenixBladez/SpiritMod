using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Terraria.ModLoader;
using Terraria;
using Terraria.ID;


namespace SpiritMod.Projectiles.Hostile
{
	public class WheezerSporeHostile : ModProjectile
	{
		int moveSpeed = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore");
		}

        public override void SetDefaults()
        {
            projectile.hostile = true;
            projectile.width = 20;
            projectile.height = 20;
            projectile.timeLeft = 1000;
            ;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.penetrate = 1;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 1);
            for (int k = 0; k < 15; k++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 42);
                Main.dust[dust].scale = .61f;
                Main.dust[dust].noGravity = true;
            }
        }
        public override bool PreAI()
        {
            var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
            foreach (var proj in list)
            {
                if (projectile != proj && proj.friendly)
                {
                    projectile.Kill();
                }
            }
            return true;
        }
        public override void AI()
        {
            int range = 650;   //How many tiles away the projectile targets NPCs
                               //int targetingMax = 20; //how many frames allowed to target nearest instead of shooting
                               //float shootVelocity = 16f; //magnitude of the shoot vector (speed of arrows shot)

            //TARGET NEAREST NPC WITHIN RANGE
            float lowestDist = float.MaxValue;
            foreach (Player player in Main.player)
            {
                //if npc is a valid target (active, not friendly, and not a critter)
                if (player.active)
                {
                    //if npc is within 50 blocks
                    float dist = projectile.Distance(player.Center);
                    if (dist / 16 < range)
                    {
                        //if npc is closer than closest found npc
                        if (dist < lowestDist)
                        {
                            lowestDist = dist;

                            //target this npc
                            projectile.ai[1] = player.whoAmI;
                        }
                    }
                }
            }

            Player target = (Main.player[(int)projectile.ai[1]] ?? new Player());
            if (target.active && projectile.Distance(target.Center) / 16 < range && projectile.timeLeft < 945)
            {
                if (projectile.Center.X >= target.Center.X && moveSpeed >= -30) // flies to players x position
                {
                    moveSpeed--;
                }

                if (projectile.Center.X <= target.Center.X && moveSpeed <= 30)
                {
                    moveSpeed++;
                }

                projectile.velocity.X = moveSpeed * 0.1f;
                projectile.velocity.Y = 1;
            }
        }
    }
}