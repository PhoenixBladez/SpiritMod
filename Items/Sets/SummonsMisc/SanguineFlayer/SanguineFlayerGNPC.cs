using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Items.Sets.SummonsMisc.SanguineFlayer
{
	public class SanguineFlayerGNPC : GlobalNPC
	{
		private int _isHooked = 0;
		public bool IsHooked 
		{
			get => _isHooked > 0;
			set => _isHooked = (value ? 3 : 0);
		}

		public int HookDamage { get; set; } = 0;

		public override void ResetEffects(NPC npc)
		{
			if (!IsHooked)
				HookDamage = 0;

			_isHooked = Math.Max(_isHooked - 1, 0);
		}

		public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
		{
			bool IsSummonDamage = projectile.minion || projectile.sentry || ProjectileID.Sets.MinionShot[projectile.type] || ProjectileID.Sets.SentryShot[projectile.type];
			if (IsHooked && IsSummonDamage)
			{
				HookDamage += damage;
			}
		}

		public override bool InstancePerEntity => true;
		protected override bool CloneNewInstances => true;

	}
}