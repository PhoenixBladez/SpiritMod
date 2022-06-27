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
			Main.projPet[Projectile.type] = true;
			Main.projFrames[base.Projectile.type] = 1;
			ProjectileID.Sets.MinionSacrificable[base.Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[base.Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Spazmamini);
			AIType = ProjectileID.Spazmamini;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.minion = true;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.netImportant = true;
			Projectile.alpha = 0;
			Projectile.penetrate = -10;
			Projectile.minionSlots = 1;
		}
		
		public override bool? CanCutTiles() {
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.penetrate == 0)
				Projectile.Kill();
			return false;
		}

		public override void AI()
		{
			bool flag64 = Projectile.type == ModContent.ProjectileType<ReachSummon>();
			Player player = Main.player[Projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (flag64) {
				if (player.dead)
				modPlayer.ReachSummon = false;
				if (modPlayer.ReachSummon)
				Projectile.timeLeft = 2;
			}
		}

		public override bool MinionContactDamage()
		{
			return true;
		}
	}
}