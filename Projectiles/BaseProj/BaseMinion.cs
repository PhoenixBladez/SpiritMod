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
		public BaseMinion(float TargettingRange, float DeaggroRange, Vector2 Size) 
		{
			this.TargettingRange = TargettingRange;
			this.DeaggroRange = DeaggroRange;
			this.Size = Size;
		}
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;

			AbstractSetStaticDefaults();
		}

		public virtual void AbstractSetStaticDefaults() { }

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1;
			Projectile.Size = Size;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;

			AbstractSetDefaults();
		}

		private NPC _targetNPC;

		public bool CanRetarget { get; set; }

		public virtual void AbstractSetDefaults() { }

		internal Player Player => Main.player[Projectile.owner];
		internal int IndexOfType => Main.projectile.Where(x => x.active && x.owner == Projectile.owner && x.type == Projectile.type && x.whoAmI < Projectile.whoAmI).Count();

		private bool _hadTarget = false;
		private bool HadTarget
		{
			get => _hadTarget;
			set
			{
				if(_hadTarget != value)
				{
					_hadTarget = value;
					Projectile.netUpdate = true;
				}
			}
		}

		public override void AI()
		{
			float maxdist = TargettingRange;
			NPC miniontarget = Projectile.OwnerMinionAttackTargetNPC;
			if (miniontarget != null && miniontarget.CanBeChasedBy(this) && (CanHit(Projectile.Center, miniontarget.Center) || HadTarget) && CanHit(Player.Center, miniontarget.Center)
				&& (miniontarget.Distance(Player.Center) <= maxdist || miniontarget.Distance(Projectile.Center) <= maxdist) && miniontarget.Distance(Player.Center) <= DeaggroRange)
				_targetNPC = miniontarget;

			else
			{
				var validtargets = Main.npc.Where(x => x != null && x.CanBeChasedBy(this) && (CanHit(Projectile.Center, x.Center) || HadTarget) && CanHit(Player.Center, x.Center)
														 && (x.Distance(Player.Center) <= maxdist || x.Distance(Projectile.Center) <= maxdist) && x.Distance(Player.Center) <= DeaggroRange);

				if (!validtargets.Contains(_targetNPC))
					_targetNPC = null;

				if (CanRetarget)
				{
					foreach (NPC npc in validtargets)
					{
						if (npc.Distance(Projectile.Center) <= maxdist)
						{
							maxdist = npc.Distance(Projectile.Center);
							_targetNPC = npc;
						}
					}
				}
			}

			CanRetarget = true;
			if (_targetNPC == null)
			{
				IdleMovement(Player);
				HadTarget = false;
			}
			else
			{
				TargettingBehavior(Player, _targetNPC);
				HadTarget = true;
			}

			int framespersecond = 1;
			int startframe = 0;
			int endframe = Main.projFrames[Projectile.type];
			if (DoAutoFrameUpdate(ref framespersecond, ref startframe, ref endframe))
				UpdateFrame(framespersecond, startframe, endframe);
		}

		private bool CanHit(Vector2 center1, Vector2 center2) => Collision.CanHit(center1, 0, 0, center2, 0, 0);

		public virtual void IdleMovement(Player player) { }

		public override bool? CanCutTiles() => false;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(HadTarget);
			writer.Write(_targetNPC == null ? -1 : _targetNPC.whoAmI);
			writer.Write(CanRetarget);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			HadTarget = reader.ReadBoolean();
			int whoamI = reader.ReadInt32();
			_targetNPC = whoamI == -1 ? null : Main.npc[_targetNPC.whoAmI];
			CanRetarget = reader.ReadBoolean();
		}

		public override bool MinionContactDamage() => true;

		public virtual void TargettingBehavior(Player player, NPC target) { }

		public virtual bool DoAutoFrameUpdate(ref int framespersecond, ref int startframe, ref int endframe) => true;

		private void UpdateFrame(int framespersecond, int startframe, int endframe)
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 60 / framespersecond)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;

				if (Projectile.frame >= endframe)
					Projectile.frame = startframe;
			}
		}
	}
}