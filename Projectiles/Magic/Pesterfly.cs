using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Pesterfly : ModProjectile
	{
		ref float AttachedNPC => ref projectile.ai[0];

		private bool canAttach = true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pesterfly");
			Main.projFrames[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.timeLeft = Main.rand.Next(280, 321);
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 2;
            projectile.tileCollide = true;
			projectile.sentry = true;
			projectile.ignoreWater = true;
			projectile.magic = true;

			projectile.ai[0] = -1;
		}

        public override void AI()
		{
			if (projectile.frameCounter++ % 10 < 5) //anim
				projectile.frame = 0;
			else
				projectile.frame = 1;

			projectile.velocity = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(2)); //Makes it feel like a fly's flying :)

			if (AttachedNPC != -1) //"leeching"
			{
				NPC npc = Main.npc[(int)AttachedNPC];
				if (npc.CanBeChasedBy()) //valid NPC
				{
					projectile.tileCollide = false;
					if (projectile.velocity.Length() > 8f)
						projectile.velocity = Vector2.Normalize(projectile.velocity) * 8f;
					projectile.velocity += Vector2.Normalize(npc.Center - projectile.Center) * 0.9f;
				}
				else //not valid NPC
				{
					AttachedNPC = -1;
					projectile.tileCollide = true;
					canAttach = true;	
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (canAttach) //self explanatory
			{
				AttachedNPC = target.whoAmI;
				canAttach = false;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = true; //go through platforms
			return true;
		}

		public override void Kill(int timeLeft) //on kill effect I literally stole from another projectile in the mod verbatim
		{
			Main.PlaySound(SoundID.Critter, (int)projectile.position.X, (int)projectile.position.Y);
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Plantera_Green, 0f, -2f, 0, default, 1.5f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != projectile.Center) 
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
			}
		}
	}
}