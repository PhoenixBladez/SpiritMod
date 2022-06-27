using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ID;
using SpiritMod.Buffs;

namespace SpiritMod.Mechanics.BoonSystem
{
	public class BoonNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public Boon currentBoon;

		public override void SetDefaults(NPC npc)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
				//TryHandlePackets();
			}

			// no weirdness while trying to setup content, please, thanks - and nothing should happen on the menu either
			if (!SpiritMod.Instance.FinishedContentSetup || Main.gameMenu || Main.LocalPlayer == null)
				return;

			if (npc.ModNPC is IBoonable || npc.type == NPCID.Medusa)
				ApplyBoon(npc);
		}

		public void ApplyBoon(NPC npc)
		{
			int chance = 15;

			if (Main.netMode == NetmodeID.SinglePlayer) //Check if any player has the boon increase buff
			{
				if (Main.LocalPlayer.HasBuff(ModContent.BuffType<OracleBoonBuff>()))
					chance = 5;
			}
			else
			{
				for (int i = 0; i < Main.maxPlayers; ++i)
				{
					Player p = Main.player[i];
					if (p.active && !p.dead && p.HasBuff(ModContent.BuffType<OracleBoonBuff>()))
					{
						chance = 5;
						break;
					}
				}
			}

			if (!Main.rand.NextBool(chance)) //Stop trying to add the boon if we don't pass the check
				return;

			currentBoon = GetBoon(npc);

			if (currentBoon == null) return;

			if (Main.netMode == NetmodeID.Server) // if we're on server, send it to the clients
			{
				// using IndexOf won't work, as a new index has been created using Activator.CreateInstance, which will have a different memory address
				// temporary work around, get all the types of LoadedBoons and get the index from there
				//int index = BoonLoader.LoadedBoons.IndexOf(currentBoon);
				int index = BoonLoader.LoadedBoonTypes.IndexOf(currentBoon.GetType());

				if (index != -1)
				{
					int npcWhoAmI = -1;
					for (int i = 0; i < 200; i++)
					{
						if (Main.npc[i] == npc)
						{
							npcWhoAmI = i;
							break;
						}
					}

					if (npcWhoAmI != -1)
					{
						SpiritMod.Instance.Logger.Debug($"Writing new boon data, index: {npcWhoAmI} boonType: {index} which is {currentBoon.GetType().Name}");

						SpiritMultiplayer.WriteToPacketAndSend(4, MessageType.BoonData, packet =>
						{
							packet.Write((ushort)npc.type);
							packet.Write((ushort)npcWhoAmI);
							packet.Write((byte)index);
						});
					}
				}
			}

			currentBoon.SetStats();
		}

		#region boon hooks
		public override void AI(NPC npc) => currentBoon?.AI();
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => currentBoon?.PostDraw(spriteBatch, color);

		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
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