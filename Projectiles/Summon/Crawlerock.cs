using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class Crawlerock : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crawlerock");
			Main.projFrames[base.projectile.type] = 1;
			ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.Homing[base.projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.BabySlime);
			projectile.width = 32;
			projectile.height = 32;
			projectile.minion = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.netImportant = true;
			aiType = ProjectileID.BabySlime;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
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
			bool flag64 = projectile.type == mod.ProjectileType("Crawlerock");
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (flag64)
			{
				if (player.dead)
					modPlayer.crawlerockMinion = false;

				if (modPlayer.crawlerockMinion)
					projectile.timeLeft =2;

			}
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

	}
}