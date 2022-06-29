using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace SpiritMod.Items
{
	public abstract class SpiritPlayerEffect
	{
		// Called in PostUpdate when the effect hasn't been refreshed
		// Use this to reset stats, etc
		public virtual void EffectRemoved(Player player) { }

		public virtual void PlayerProcessTriggers(Player player, TriggersSet triggersSet) { }
		public virtual void PlayerModifyWeaponDamage(Player player, Item item, ref float add, ref float mult, ref float flat) { }
		public virtual void PlayerOnHitAnything(Player player, float x, float y, Entity victim) { }
		public virtual void PlayerOnHitNPC(Player player, Item item, NPC target, int damage, float knockback, bool crit) { }
		public virtual void PlayerOnHitNPCWithProj(Player player, Projectile proj, NPC target, int damage, float knockback, bool crit) { }
		public virtual bool PlayerPreHurt(Player player, bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) => true;
		public virtual void PlayerHurt(Player player, bool pvp, bool quiet, double damage, int hitDirection, bool crit) { }
		public virtual void PlayerPostHurt(Player player, bool pvp, bool quiet, double damage, int hitDirection, bool crit) { }
		public virtual bool PlayerPreKill(Player player, double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) => true;
		public virtual void PlayerPreUpdate(Player player) { }
		public virtual void PlayerUpdateBadLifeRegen(Player player) { }
		public virtual void PlayerUpdateLifeRegen(Player player) { }
		public virtual void PlayerPostUpdateEquips(Player player) { }
		public virtual void PlayerPostUpdate(Player player) { }
		public virtual void PlayerModifyHitNPC(Player player, Item item, NPC target, ref int damage, ref float knockback, ref bool crit) { }
		public virtual void PlayerDrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright) { }

		///<summary>Only runs if this is being used through an accessory.</summary>
		public virtual void ItemUpdateAccessory(Player player, bool hideVisual) { }
		///<summary>Only runs if this is being used through an armor set.</summary>
		public virtual void ItemUpdateArmorSet(Player player) { }

		public virtual void TileFloorVisuals(int type, Player player) { }
	}
}
