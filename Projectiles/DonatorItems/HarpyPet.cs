using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using SpiritMod.Items.DonatorItems;
using Microsoft.Xna.Framework;

namespace SpiritMod.Projectiles.DonatorItems
{
	class HarpyPet : ModProjectile
	{
		public static readonly int _type;

		private const float FOV = (float)System.Math.PI / 2;
		private const float Max_Range = 16 * 30;
		private const float Spread = (float)System.Math.PI / 9;
		private const int Damage = 15;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mini Harpy");
			Main.projFrames[projectile.type] = 3;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 20;
			projectile.height = 20;
			projectile.aiStyle = 26;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft *= 5;
			aiType = ProjectileID.BabyHornet;
		}

		private float Timer
		{
			get { return projectile.localAI[1]; }
			set { projectile.localAI[1] = value; }
		}

		private int animationCounter;
		private int frame;
		public override void AI()
		{
			if (++animationCounter >= 5)
			{
				animationCounter = 0;
				if (++frame >= Main.projFrames[_type])
					frame = 0;
			}
			projectile.frameCounter = 0;
			projectile.frame = frame;

			var owner = Main.player[projectile.owner];
			if (owner.active && owner.HasBuff(HarpyPetBuff._type))
				projectile.timeLeft = 2;

			if (projectile.owner != Main.myPlayer)
				return;

			if (Timer > 0)
			{
				--Timer;
				return;
			}
			
			float direction;
			if (projectile.direction < 0)
				direction = FOVHelper.POS_X_DIR + projectile.rotation;
			else
				direction = FOVHelper.NEG_X_DIR - projectile.rotation;

			var origin = projectile.Center;
			var fov = new FOVHelper();
			fov.adjustCone(origin, FOV, direction);
			float maxDistSquared = Max_Range * Max_Range;
			for (int i = 0; i < Main.maxNPCs; ++i)
			{
				NPC npc = Main.npc[i];
				Vector2 npcPos = npc.Center;
				if (npc.CanBeChasedBy() &&
					fov.isInCone(npcPos) &&
					Vector2.DistanceSquared(origin, npcPos) < maxDistSquared &&
					Collision.CanHitLine(origin, 0, 0, npc.position, npc.width, npc.height))
				{
					ShootFeathersAt(npcPos);
					Timer = 140;
					break;
				}
			}
		}

		private void ShootFeathersAt(Vector2 target)
		{
			var origin = projectile.Center;
			var direction = target - origin;
			direction = direction.SafeNormalize(Vector2.UnitX);
			direction *= 3f;
			Projectile.NewProjectile(origin, direction.RotatedBy(Spread), HarpyPetFeather._type, Damage, 0, projectile.owner);
			Projectile.NewProjectile(origin, direction.RotatedBy(-Spread), HarpyPetFeather._type, Damage, 0, projectile.owner);
			Projectile.NewProjectile(origin, direction, HarpyPetFeather._type, Damage, 0, projectile.owner);
		}
	}
}
