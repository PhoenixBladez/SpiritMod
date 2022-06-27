using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.BaseProj
{
	/// <summary>
	/// Handles logic for exploding upon death, not multi-hitting an npc, becoming hostile to players, and doing the correct amount of damage to players.
	/// </summary>
	public abstract class BaseRocketProj : ModProjectile
	{
		private int _hitNPC = -1;

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (_hitNPC == -1)
				_hitNPC = target.whoAmI;

			AbstractHitNPC(target, damage, knockback, crit);
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (target.townNPC)
				return false;

			if (target.whoAmI == _hitNPC)
				return false;

			return base.CanHitNPC(target);
		}

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit) => damage = NPCUtils.ToActualDamage(damage);

		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit) => damage = NPCUtils.ToActualDamage(damage);

		public override void Kill(int timeLeft)
		{
			Projectile.hostile = true;
			ProjectileExtras.Explode(Projectile.whoAmI, ExplosionRange, ExplosionRange, delegate
			{
				if (!Main.dedServ)
					ExplodeEffect();
			});
		}

		/// <summary>
		/// Use in cases where OnHitNPC would be used, due to BaseRocketProj's implementation of OnHitNPC
		/// </summary>
		/// <param name="target"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		/// <param name="crit"></param>
		public virtual void AbstractHitNPC(NPC target, int damage, float knockback, bool crit) { }

		/// <summary>
		/// Use for client-side visual and sound effects after the projectile explodes.
		/// </summary>
		public virtual void ExplodeEffect() { }

		/// <summary>
		/// The width and height of the projectile, in pixels, after exploding.
		/// </summary>
		public virtual int ExplosionRange => 100;

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(_hitNPC);

		public override void ReceiveExtraAI(BinaryReader reader) => _hitNPC = reader.ReadInt32();
	}
}