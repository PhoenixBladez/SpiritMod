using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Pesterfly : ModProjectile
	{
		ref float AttachedNPC => ref Projectile.ai[0];

		private bool canAttach = true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pesterfly");
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.timeLeft = Main.rand.Next(280, 321);
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 2;
            Projectile.tileCollide = true;
			Projectile.sentry = true;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Magic;

			Projectile.ai[0] = -1;
		}

        public override void AI()
		{
			if (Projectile.frameCounter++ % 10 < 5) //anim
				Projectile.frame = 0;
			else
				Projectile.frame = 1;

			Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(2)); //Makes it feel like a fly's flying :)

			if (AttachedNPC != -1) //"leeching"
			{
				NPC npc = Main.npc[(int)AttachedNPC];
				if (npc.CanBeChasedBy()) //valid NPC
				{
					Projectile.tileCollide = false;
					if (Projectile.velocity.Length() > 8f)
						Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 8f;
					Projectile.velocity += Vector2.Normalize(npc.Center - Projectile.Center) * 0.9f;
				}
				else //not valid NPC
				{
					AttachedNPC = -1;
					Projectile.tileCollide = true;
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

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = true; //go through platforms
			return true;
		}

		public override void Kill(int timeLeft) //on kill effect I literally stole from another projectile in the mod verbatim
		{
			SoundEngine.PlaySound(SoundID.Critter, (int)Projectile.position.X, (int)Projectile.position.Y);
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Plantera_Green, 0f, -2f, 0, default, 1.5f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != Projectile.Center) 
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
			}
		}
	}
}