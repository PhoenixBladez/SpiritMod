using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class BeetleMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			DisplayName.SetDefault("Beetle");
			Main.projFrames[projectile.type] = 5;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.OneEyedPirate);
			projectile.width = 32;
			projectile.height = 20;
			projectile.minion = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.netImportant = true;
			aiType = ProjectileID.OneEyedPirate;
			projectile.alpha = 0;
			projectile.penetrate = -1;
			projectile.minionSlots = 1;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.penetrate == 0)
				projectile.Kill();

			return false;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 1)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 5)
					projectile.frame = 0;

			}

			bool flag64 = projectile.type == mod.ProjectileType("BeetleMinion");
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (flag64)
			{
				if (player.dead)
					modPlayer.beetleMinion = false;

				if (modPlayer.beetleMinion)
					projectile.timeLeft =2;

			}
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

	}
}