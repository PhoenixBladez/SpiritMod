using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Mechanics.BoonSystem
{
	public abstract class Boon
	{
		public NPC npc;

		public virtual bool CanApply => false;

		#region boon hooks

		public virtual void SpawnIn() { } 

		public virtual void PreDraw(SpriteBatch spriteBatch, Color lightColor) { }

		public virtual void PostDraw(SpriteBatch spriteBatch, Color lightColor) { }
		public virtual void AI() { }

		public virtual void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit) { }

		public virtual void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit) { }

		public virtual void OnDeath() { }

		public virtual void Death() { }

		#endregion
	}
}