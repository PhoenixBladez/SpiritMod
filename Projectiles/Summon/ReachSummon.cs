using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class ReachSummon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Briar Spirit");
			Main.projPet[projectile.type] = true;
			Main.projFrames[base.projectile.type] = 1;
			ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.Homing[base.projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Spazmamini);
			aiType = ProjectileID.Spazmamini;
			projectile.width = 24;
			projectile.height = 24;
			projectile.minion = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.netImportant = true;
			projectile.alpha = 0;
			projectile.penetrate = -10;
			projectile.minionSlots = 1;
		}
		
		public override bool? CanCutTiles() {
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.penetrate == 0)
				projectile.Kill();
			return false;
		}

		public override void AI()
		{
			bool flag64 = projectile.type == ModContent.ProjectileType<ReachSummon>();
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (flag64) {
				if (player.dead)
				modPlayer.ReachSummon = false;
				if (modPlayer.ReachSummon)
				projectile.timeLeft = 2;
			}
		}

		public override bool MinionContactDamage()
		{
			return true;
		}
	}
}