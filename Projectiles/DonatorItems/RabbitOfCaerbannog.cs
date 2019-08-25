using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using SpiritMod.Items.DonatorItems;

namespace SpiritMod.Projectiles.DonatorItems
{
	class RabbitOfCaerbannog : ModProjectile
	{
		public static readonly int _type;
		int frame2 = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rabbit of Caerbannog");
			Main.projFrames[projectile.type] = 8;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.OneEyedPirate);
			projectile.width = 48;
			projectile.height = 36;
			projectile.minion = true;
			projectile.friendly = true;
			projectile.damage = 30;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.netImportant = true;
			aiType = 0;
			projectile.alpha = 0;
			projectile.penetrate = -1;
			projectile.minionSlots = 1;
		}
		
		public override void AI()
		{
			var owner = Main.player[projectile.owner];
			if (owner.active && owner.HasBuff(RabbitOfCaerbannogBuff._type))
				projectile.timeLeft = 2;
			
			
			
			if (projectile.velocity.X != 0)
			{
			projectile.frame = frame2;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 7)
			{
				frame2 = (frame2 + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}
			}
			else
			{
				projectile.frame = 0;
			}
		}
		public override bool MinionContactDamage()
		{
			return true;
		}

	}
}
