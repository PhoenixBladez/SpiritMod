using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Overseer
{
	public class SpiritPortal : ModProjectile
	{
		bool start = true;
		// USE THIS DUST: 261

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Portal");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 360;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;

			projectile.penetrate = -1;

			projectile.timeLeft = 700;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 6);
			NPC parent = Main.npc[NPC.FindFirstNPC(mod.NPCType("Overseer"))];
			Player player = Main.player[parent.target];
			Vector2 direction8 = player.Center - projectile.Center;
			direction8.Normalize();
			direction8.X *= 22f;
			direction8.Y *= 22f;

			int amountOfProjectiles = Main.rand.Next(5, 7);
			for (int i = 0; i < amountOfProjectiles; ++i)
			{
				float A = (float)Main.rand.Next(-250, 250) * 0.01f;
				float B = (float)Main.rand.Next(-250, 250) * 0.01f;
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction8.X + A, direction8.Y + B, mod.ProjectileType("SpiritShard"), 80, 1, Main.myPlayer, 0, 0);
			}
		}

		public override bool PreAI()
		{
			if (start)
			{
				for (int num621 = 0; num621 < 55; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 206, 0f, 0f, 206, default(Color), 2f);
				}
				projectile.ai[1] = projectile.ai[0];
				start = false;
			}
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 206, 0f, 0f, 206, default(Color), 2f);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 206, 0f, 0f, 206, default(Color), 2f);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 206, 0f, 0f, 206, default(Color), 2f);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 206, 0f, 0f, 206, default(Color), 2f);
			projectile.rotation = projectile.rotation + 3f;
			//projectile.rotation = projectile.rotation + 3f;
			//Making player variable "p" set as the projectile's owner
			float lowestDist = float.MaxValue;
			NPC parent = Main.npc[NPC.FindFirstNPC(mod.NPCType("Overseer"))];
			Player player = Main.player[parent.target]; // 
			if ((projectile.ai[1] / 2) % 75 == 1)
			{
				Vector2 dir = player.Center - projectile.Center;
				dir.Normalize();
				dir *= 14;
				int spiritdude = NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, mod.NPCType("CaptiveSpirit"), parent.target, 0, 0, 0, -1);
				NPC Spirits = Main.npc[spiritdude];
				Spirits.ai[0] = dir.X;
				Spirits.ai[1] = dir.Y;
			}
			//Factors for calculations
			double deg = (double)projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
			double rad = deg * (Math.PI / 180); //Convert degrees to radians
			double dist = 500; //Distance away from the player

			/*Position the projectile based on where the player is, the Sin/Cos of the angle times the /
    		/distance for the desired distance away from the player minus the projectile's width   /
    		/and height divided by two so the center of the projectile is at the right place.     */
			projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width / 2;
			projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height / 2;

			//Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
			projectile.ai[1] += 2f;

			return false;
		}


	}
}
