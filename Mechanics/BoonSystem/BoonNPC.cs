using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SpiritMod.Mechanics.BoonSystem
{
	public class BoonNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public Boon currentBoon;



		public override void SetDefaults(NPC npc)
		{
			if (npc.modNPC is IBoonable modNPC)
			{
				currentBoon = GetBoon(npc);
			}
		}

		#region boon hooks
		public override void AI(NPC npc) => currentBoon?.AI();
		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color color)
		{
			currentBoon?.PreDraw(spriteBatch, color);
			return true;
		}
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color color)
		{
			currentBoon?.PostDraw(spriteBatch, color);
		}

		public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
		{
			currentBoon?.OnHitByProjectile(projectile, damage, knockback, crit);
			if (npc.life <= 0)
				currentBoon?.OnDeath();
		}

		public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
		{
			currentBoon?.OnHitByItem(player, item, damage, knockback, crit);
			if (npc.life <= 0)
				currentBoon?.OnDeath();
		}
		#endregion

		private static Boon GetBoon(NPC npc)
		{
			List<Boon> possibleBoons = new List<Boon>();

			foreach (Boon boon in BoonLoader.LoadedBoons)
			{
				if (boon.CanApply)
				{
					possibleBoons.Add(boon);
				}
			}

			if (possibleBoons.Count == 0)
				return null;

			Boon referenceBoon = possibleBoons[Main.rand.Next(possibleBoons.Count)];
			Boon ret = Activator.CreateInstance(referenceBoon.GetType()) as Boon;

			ret.npc = npc;
			ret.SpawnIn();
			return ret;
		}
	}
}