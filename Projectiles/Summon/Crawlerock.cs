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
			Main.projPet[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			AIType = ProjectileID.BabySlime;
			Projectile.CloneDefaults(ProjectileID.BabySlime);
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.minion = true;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.netImportant = true;
			Projectile.penetrate = -1;
			Projectile.minionSlots = 1;
			Projectile.alpha = 0;
		}

		public override bool? CanCutTiles() => false;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.penetrate == 0)
				Projectile.Kill();
			return false;
		}

		public override void AI()
		{
			bool flag64 = Projectile.type == ModContent.ProjectileType<Crawlerock>();
			Player player = Main.player[Projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (flag64)
			{
				if (player.dead)
					modPlayer.crawlerockMinion = false;
				if (modPlayer.crawlerockMinion)
					Projectile.timeLeft = 2;
			}

			Projectile.spriteDirection = -Projectile.direction;
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 6)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame > 3)
				Projectile.frame = 0;
		}

		public override bool MinionContactDamage() => true;
	}
}