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
			Main.projFrames[Projectile.type] = 8;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			AIType = ProjectileID.BabySlime;
			Projectile.CloneDefaults(ProjectileID.BabySlime);
			Projectile.width = 48;
			Projectile.height = 36;
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
			bool flag64 = Projectile.type == ModContent.ProjectileType<RabbitOfCaerbannog>();
			Player player = Main.player[Projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (flag64) {
				if (player.dead)
				modPlayer.rabbitMinion = false;
				if (modPlayer.rabbitMinion)
				Projectile.timeLeft = 2;
			}

			if (Projectile.velocity.X != 0) {
				Projectile.frame = frame2;
				Projectile.frameCounter++;
				if (Projectile.frameCounter >= 7) {
					frame2 = (frame2 + 1) % Main.projFrames[Projectile.type];
					Projectile.frameCounter = 0;
				}
			}
			else {
				Projectile.frame = 0;
			}
		}

		public override bool MinionContactDamage() => true;
	}
}
