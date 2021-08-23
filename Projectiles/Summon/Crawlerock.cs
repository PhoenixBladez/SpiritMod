using Microsoft.Xna.Framework;
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
			Main.projPet[projectile.type] = true;
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			aiType = ProjectileID.BabySlime;
			projectile.CloneDefaults(ProjectileID.BabySlime);
			projectile.width = 32;
			projectile.height = 32;
			projectile.minion = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.netImportant = true;
			projectile.penetrate = -1;
			projectile.minionSlots = 1;
			projectile.alpha = 0;
		}

		public override bool? CanCutTiles() => false;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.penetrate == 0)
				projectile.Kill();
			return false;
		}

		public override void AI()
		{
			bool flag64 = projectile.type == ModContent.ProjectileType<Crawlerock>();
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (flag64)
			{
				if (player.dead)
					modPlayer.crawlerockMinion = false;
				if (modPlayer.crawlerockMinion)
					projectile.timeLeft = 2;
			}

			projectile.spriteDirection = -projectile.direction;
			projectile.frameCounter++;
			if (projectile.frameCounter > 6)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame > 3)
				projectile.frame = 0;
		}

		public override bool MinionContactDamage() => true;
	}
}