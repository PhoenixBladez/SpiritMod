using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class BabyClamper : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Baby Clamper");
			Main.projFrames[base.projectile.type] = 1;
			ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.Homing[base.projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.BabySlime);
			projectile.width = 16;
			projectile.height = 16;
			projectile.minion = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.netImportant = true;
			aiType = ProjectileID.BabySlime;
			projectile.alpha = 0;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.minionSlots = 0;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.penetrate == 0)
				projectile.Kill();

			return false;
		}

		public override void AI()
		{
			MyPlayer mp = Main.player[projectile.owner].GetModPlayer<MyPlayer>(mod);
			if (mp.oceanSet && !mp.player.dead)
				projectile.timeLeft = 2;

		}

		public override bool MinionContactDamage()
		{
			return true;
		}

	}
}