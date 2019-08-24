using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Pet
{
	public class SaucerPet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Support Saucer");
			Main.projFrames[projectile.type] = 5;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ZephyrFish);
			aiType = ProjectileID.ZephyrFish;
			projectile.width = 40;
			projectile.height = 30;
		}

		public override bool PreAI()
		{
			int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0, -1f, 0, default(Color), 1f);
			Main.dust[d].scale *= 0.5f;

			Player player = Main.player[projectile.owner];
			player.zephyrfish = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.dead)
				modPlayer.saucerPet = false;

			if (modPlayer.saucerPet)
				projectile.timeLeft = 2;
		}

	}
}