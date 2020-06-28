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
			Main.projFrames[projectile.type] = 27;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 45;
			projectile.height = 62;
			projectile.scale = 0.85f;
			projectile.aiStyle = 144;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.manualDirectionChange = true;
			projectile.timeLeft *= 5;
			aiType = ProjectileID.DD2PetGato;
		}

		private int animationCounter;
		private int frame;
		public override void AI()
		{
			if(++animationCounter >= 6) {
				animationCounter = 0;
				if(++frame >= Main.projFrames[projectile.type]) {
					if(Main.rand.Next(2) == 0)
						frame = 0;
					else
						frame = 9;
				}
			}
			projectile.frameCounter = 2;
			projectile.frame = frame;
			var owner = Main.player[projectile.owner];
			if(owner.active && owner.HasBuff(ModContent.BuffType<LoomingPresence>()))
				projectile.timeLeft = 2;
		}
	}
}
