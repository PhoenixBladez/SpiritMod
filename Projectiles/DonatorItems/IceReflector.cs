using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	public class IceReflector : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Reflector");
		}

		public override void SetDefaults()
		{
			projectile.width = 44;
			projectile.height = 60;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 360;
		}

		public override bool PreAI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 135, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].scale = 0.5f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = true;

			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.Center = new Vector2(player.Center.X + (player.direction > 0 ? 0 : 0), player.position.Y + 30);   // I dont know why I had to set it to -60 so that it would look right   (change to -40 to 40 so that it's on the floor)
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
			foreach (var proj in list)
			{
				if (projectile != proj && !proj.friendly)
					proj.Kill();

				projectile.localAI[0] += 1f;
				if (projectile.localAI[0] >= 10f)
				{
					projectile.localAI[0] = 0f;
					int num416 = 0;
					int num417 = 0;
					float num418 = 0f;
					int num419 = projectile.type;
					for (int num420 = 0; num420 < 1000; num420++)
					{
						if (Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f)
						{
							num416++;
							if (Main.projectile[num420].ai[1] > num418)
							{
								num417 = num420;
								num418 = Main.projectile[num420].ai[1];
							}
						}
					}
					if (num416 > 2)
					{
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						return;
					}
				}
			}
		}

	}
}
