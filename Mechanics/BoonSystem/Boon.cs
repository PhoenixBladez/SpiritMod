using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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

		#region helpers
		protected void DrawSigil(SpriteBatch spriteBatch, Texture2D tex, float counter)
		{
			float progress = counter % 1;
			float transparency = (float)Math.Pow(1 - progress, 2);
			float scale = 1 + progress;

			spriteBatch.Draw(tex, npc.Center - Main.screenPosition, null, Color.White * transparency, 0, tex.Size() / 2, npc.scale * scale, SpriteEffects.None, 0f);

			spriteBatch.Draw(tex, npc.Center - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, npc.scale, SpriteEffects.None, 0f);
		}
		#endregion
	}
}