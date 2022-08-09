using SpiritMod.Items.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	class DemonicBlob : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wishbone");
			Main.projFrames[Projectile.type] = 27;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 45;
			Projectile.height = 62;
			Projectile.scale = 0.85f;
			Projectile.aiStyle = 144;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.manualDirectionChange = true;
			Projectile.timeLeft *= 5;
			AIType = ProjectileID.DD2PetGato;
		}

		private int animationCounter;
		private int frame;
		public override void AI()
		{
			if (++animationCounter >= 6) {
				animationCounter = 0;
				if (++frame >= Main.projFrames[Projectile.type]) {
					if (Main.rand.NextBool(2))
						frame = 0;
					else
						frame = 9;
				}
			}
			Projectile.frameCounter = 2;
			Projectile.frame = frame;
			var owner = Main.player[Projectile.owner];
			if (owner.active && owner.HasBuff(ModContent.BuffType<LoomingPresence>()))
				Projectile.timeLeft = 2;
		}
	}
}
