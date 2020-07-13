using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace SpiritMod.Items
{
	public abstract class SpiritProjectileEffect
	{
		public virtual bool ProjectilePreAI(Projectile projectile) => true;
		public virtual void ProjectileOnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit) { }
	}
}
