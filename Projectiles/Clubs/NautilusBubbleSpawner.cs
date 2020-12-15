using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Projectiles.Clubs
{
	public class NautilusBubbleSpawner : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nautilus Bubble");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 30;
			projectile.height = 30;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.damage = 1;
			projectile.penetrate = -1;
            projectile.hide = true;
			projectile.alpha = 255;
            projectile.timeLeft = 75;
			projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.melee = true;
		}

		//projectile.ai[0]: how many more pillars. Each one is one less
		//projectile.ai[1]: 0: center, -1: going left, 1: going right
		bool activated = false;
		float startposY = 0;
		public override bool PreAI()
        {
            if (startposY == 0)
            {
                startposY = projectile.position.Y;
                if (Main.tile[(int)projectile.Center.X / 16, (int)(projectile.Center.Y / 16)].collisionType == 1)
                {
                    projectile.active = false;
                }
            }
            projectile.velocity.X = 0;
            if (!activated)
            {
                projectile.velocity.Y = 24;
				if (projectile.timeLeft < 58)
                {
                    projectile.Kill();
                }
            }
            else
            {
                if (projectile.timeLeft % 15 == 0)
                {
                    int proj = Terraria.Projectile.NewProjectile(projectile.Center.X + Main.rand.Next(-15, 15), projectile.Center.Y + 6, 0, Main.rand.NextFloat(-2f, -1f), ModContent.ProjectileType<NautilusBubbleProj>(), projectile.damage / 4, projectile.owner, 0, 0f);
                    Main.projectile[proj].scale = Main.rand.NextFloat(.8f, 1f);
                    Main.projectile[proj].timeLeft = Main.rand.Next(90, 110);
                }
            }
            return false;
		}
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.Y != projectile.velocity.Y && !activated) {
				startposY = projectile.position.Y;
				activated = true;
			}
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}

	}
}