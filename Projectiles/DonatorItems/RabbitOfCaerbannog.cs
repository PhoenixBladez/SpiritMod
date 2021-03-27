using Microsoft.Xna.Framework;
using SpiritMod.Items.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	class RabbitOfCaerbannog : ModProjectile
	{
		int frame2 = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rabbit of Caerbannog");
			Main.projFrames[projectile.type] = 8;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			aiType = ProjectileID.BabySlime;
			projectile.CloneDefaults(ProjectileID.BabySlime);
			projectile.width = 48;
			projectile.height = 36;
			projectile.minion = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.netImportant = true;
			projectile.penetrate = -1;
			projectile.minionSlots = 1;
			projectile.alpha = 0;
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
			bool flag64 = projectile.type == ModContent.ProjectileType<RabbitOfCaerbannog>();
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (flag64) {
				if (player.dead)
				modPlayer.rabbitMinion = false;
				if (modPlayer.rabbitMinion)
				projectile.timeLeft = 2;
			}

			if (projectile.velocity.X != 0) {
				projectile.frame = frame2;
				projectile.frameCounter++;
				if (projectile.frameCounter >= 7) {
					frame2 = (frame2 + 1) % Main.projFrames[projectile.type];
					projectile.frameCounter = 0;
				}
			}
			else {
				projectile.frame = 0;
			}
			
			float distanceFromTarget = 700f;
			Vector2 targetCenter = projectile.position;
			bool foundTarget = false;
			
			if (player.HasMinionAttackTargetNPC) {
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, projectile.Center);
				if (between < 2000f) {
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}
		}
		
		public override bool MinionContactDamage()
		{
			return true;
		}

	}
}
