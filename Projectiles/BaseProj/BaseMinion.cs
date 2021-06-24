using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.BaseProj
{
	public abstract class BaseMinion : ModProjectile
	{
		private readonly float TargettingRange;
		private readonly float DeaggroRange;
		private readonly Vector2 Size;
		private readonly bool ContactDamage;
		public BaseMinion(float TargettingRange, float DeaggroRange, Vector2 Size, bool ContactDamage = true) 
		{
			this.TargettingRange = TargettingRange;
			this.DeaggroRange = DeaggroRange;
			this.Size = Size;
			this.ContactDamage = ContactDamage;
		}
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			Main.projPet[projectile.type] = true;

			AbstractSetStaticDefaults();
		}

		public virtual void AbstractSetStaticDefaults() { }

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.Size = Size;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;

			AbstractSetDefaults();
		}

		public virtual void AbstractSetDefaults() { }

		internal Player Player => Main.player[projectile.owner];
		internal int IndexOfType => Main.projectile.Where(x => x.active && x.owner == projectile.owner && x.type == projectile.type && x.whoAmI < projectile.whoAmI).Count();

		private bool _hadTarget = false;
		private bool HadTarget
		{
			get => _hadTarget;
			set
			{
				if(_hadTarget != value)
				{
					_hadTarget = value;
					projectile.netUpdate = true;
				}
			}
		}

		public override void AI()
		{
			NPC target = null;
			float maxdist = TargettingRange;
			NPC miniontarget = projectile.OwnerMinionAttackTargetNPC;
			if (miniontarget != null && miniontarget.CanBeChasedBy(this) && (CanHit(projectile.Center, miniontarget.Center) || HadTarget) && CanHit(Player.Center, miniontarget.Center)
				&& (miniontarget.Distance(Player.Center) <= maxdist || miniontarget.Distance(projectile.Center) <= maxdist) && miniontarget.Distance(Player.Center) <= DeaggroRange)
				target = miniontarget;

			else
			{
				var validtargets = Main.npc.Where(x => x != null && x.CanBeChasedBy(this) && (CanHit(projectile.Center, x.Center) || HadTarget) && CanHit(Player.Center, x.Center)
														 && (x.Distance(Player.Center) <= maxdist || x.Distance(projectile.Center) <= maxdist) && x.Distance(Player.Center) <= DeaggroRange);

				foreach (NPC npc in validtargets)
				{
					if (npc.Distance(projectile.Center) <= maxdist)
					{
						maxdist = npc.Distance(projectile.Center);
						target = npc;
					}
				}
			}
			if (target == null)
			{
				IdleMovement(Player);
				HadTarget = false;
			}
			else
			{
				TargettingBehavior(Player, target);
				HadTarget = true;
			}

			int framespersecond = 1;
			if (DoAutoFrameUpdate(ref framespersecond))
				UpdateFrame(framespersecond);
		}

		private bool CanHit(Vector2 center1, Vector2 center2) => Collision.CanHit(center1, 0, 0, center2, 0, 0);

		public virtual void IdleMovement(Player player) { }

		public override bool? CanCutTiles() => false;

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(HadTarget);
		public override void ReceiveExtraAI(BinaryReader reader) => HadTarget = reader.ReadBoolean();

		public override bool MinionContactDamage() => ContactDamage;

		public virtual void TargettingBehavior(Player player, NPC target) { }

		public virtual bool DoAutoFrameUpdate(ref int framespersecond) => true;

		private void UpdateFrame(int framespersecond)
		{
			projectile.frameCounter++;
			if (projectile.frameCounter > 60 / framespersecond)
			{
				projectile.frameCounter = 0;
				projectile.frame++;

				if (projectile.frame >= Main.projFrames[projectile.type])
					projectile.frame = 0;
			}
		}
	}
}