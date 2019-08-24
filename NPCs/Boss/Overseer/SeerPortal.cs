using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Overseer
{
	public class SeerPortal : ModProjectile
	{
		// USE THIS DUST: 261
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Portal");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 360;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;

			projectile.penetrate = -1;

			projectile.timeLeft = 120;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 206, 0f, 0f, 206, default(Color), 2f);
			Main.dust[dust].noGravity = true;
		}

		public override void Kill(int timeLeft)
		{
			projectile.rotation += 0.2f;
			Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 6);
			NPC parent = Main.npc[NPC.FindFirstNPC(mod.NPCType("Overseer"))];
			for (int J = 0; J < 20; J++)
			{
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 206, 0f, 0f, 206, default(Color), 2f);
				Dust.NewDust(new Vector2(parent.position.X, parent.position.Y), parent.width, parent.height, 206, 0f, 0f, 206, default(Color), 2f);

			}
			parent.position = projectile.position;
		}

	}
}
