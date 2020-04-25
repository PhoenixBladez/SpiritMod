using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Dragon
{
    public class DragonBodyOne : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jade Dragon");
		}
        public override void SetDefaults()
        {
            projectile.penetrate = 600;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.timeLeft = 225;
			projectile.damage = 13;
            //projectile.extraUpdates = 1;
			projectile.width = projectile.height = 32;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;

        }
		public override bool PreAI()
		{
			 projectile.spriteDirection = 1;
			if (projectile.ai[0] > 0)
			{
			 projectile.spriteDirection = 0;
			}
			if (Main.netMode != 1)
			{
				if (!Main.projectile[(int)projectile.ai[1]].active)
				{
					projectile.timeLeft = 0;
					projectile.active = false;
					// NetMessage.SendData(28, -1, -1, "", projectile.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
				}
			}

			if (projectile.ai[1] < (double)Main.projectile.Length)
			{
				// We're getting the center of this projectile.
				Vector2 projectileCenter = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				// Then using that center, we calculate the direction towards the 'parent projectile' of this projectile.
				float dirX = Main.projectile[(int)projectile.ai[1]].position.X + (float)(Main.projectile[(int)projectile.ai[1]].width / 2) - projectileCenter.X;
				float dirY = Main.projectile[(int)projectile.ai[1]].position.Y + (float)(Main.projectile[(int)projectile.ai[1]].height / 2) - projectileCenter.Y;
				// We then use Atan2 to get a correct rotation towards that parent projectile.
				projectile.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
				// We also get the length of the direction vector.
				float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
				// We calculate a new, correct distance.
				float dist = (length - (float)projectile.width) / length;
				float posX = dirX * dist;
				float posY = dirY * dist;

				// Reset the velocity of this projectile, because we don't want it to move on its own
				projectile.velocity = Vector2.Zero;
				// And set this projectiles position accordingly to that of this projectiles parent projectile.
				projectile.position.X = projectile.position.X + posX;
				projectile.position.Y = projectile.position.Y + posY;
			}
			return false;
		}
	}
}