using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Pet
{
	public class ThrallPet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lil' Leonardo");
			Main.projFrames[projectile.type] = 7;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.BabySnowman);
			aiType = ProjectileID.BabySnowman;
			projectile.height = 40;
			projectile.width = 20;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.snowman = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.dead)
				modPlayer.thrallPet = false;

			if (modPlayer.thrallPet)
				projectile.timeLeft = 2;

		}

	}
}