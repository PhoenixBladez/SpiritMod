using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ID;

namespace SpiritMod.Mechanics.BoonSystem
{
	public class BoonNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public Boon currentBoon;

		public override void SetDefaults(NPC npc)
		{
			if (npc.modNPC is IBoonable)
			{
				currentBoon = GetBoon(npc);

				if (Main.netMode == NetmodeID.MultiplayerClient) //Sync boon
				{
					int index = BoonLoader.LoadedBoons.IndexOf(currentBoon);
					if (index != -1)
						SpiritMod.WriteToPacket(SpiritMod.Instance.GetPacket(4), (byte)MessageType.BoonData, (ushort)npc.whoAmI, (byte)index).Send();
				}
			}
		}

		#region boon hooks
		public override void AI(NPC npc) => currentBoon?.AI();
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color color) => currentBoon?.PostDraw(spriteBatch, color);

		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color color)
		{
			currentBoon?.PreDraw(spriteBatch, color);
			return true;
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
			/*if (Main.rand.Next(5) != 1)
				return null;*/
			List<Boon> possibleBoons = new List<Boon>();

			foreach (Boon boon in BoonLoader.LoadedBoons)
			{
				if (boon.CanApply)
					possibleBoons.Add(boon);
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