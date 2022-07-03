using Terraria;
using Terraria.ID;

namespace SpiritMod.Projectiles.Summon
{
	public class PhantomMinion : PhantomMinionINFO
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom");
			Main.projFrames[Projectile.type] = 12;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 44;
			Projectile.height = 44;
			Projectile.friendly = true;
			Main.projPet[Projectile.type] = true;
			Projectile.minion = true;
			Projectile.netImportant = true;
			Projectile.minionSlots = 0;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 18000;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			inertia = 30f;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.aiStyle = 54;
			Projectile.damage = 50;
			Lighting.AddLight((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), 1f, 1f, 10f);
		}

		public override bool MinionContactDamage()
		{
			if (Projectile.frame == 6)
				return true;

			return false;
		}

		public override void CheckActive()
		{
			Player player = Main.player[Projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.dead)
				modPlayer.Phantom = false;

			if (modPlayer.Phantom)
				Projectile.timeLeft = 2;

		}

		public override void CreateDust()
		{
			Lighting.AddLight((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), 1f, 1f, 10f);
		}

		public override void SelectFrame()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 12) {
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 12;
			}
		}

	}
}