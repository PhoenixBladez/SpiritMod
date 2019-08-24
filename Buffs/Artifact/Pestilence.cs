using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Artifact
{
	public class Pestilence : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Pestilence");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>(mod).pestilence = true;
			if (Main.rand.Next(6) == 0)
			{
				int num1 = Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("Pestilence"));
				Main.dust[num1].scale = 1.9f;
				Main.dust[num1].velocity *= 2f;
				Main.dust[num1].noGravity = true;
				float maxHomeDistance = 100f;
				int buffTime = npc.buffTime[buffIndex];
				for (int npcFinder = 0; npcFinder < 200; ++npcFinder)
				{
					if (!Main.npc[npcFinder].boss && !Main.npc[npcFinder].townNPC)
					{

						float num12 = Main.npc[npcFinder].position.X + (float)(Main.npc[npcFinder].width / 2);
						float num22 = Main.npc[npcFinder].position.Y + (float)(Main.npc[npcFinder].height / 2);
						float num32 = Math.Abs(npc.position.X + (float)(npc.width / 2) - num12) + Math.Abs(npc.position.Y + (float)(npc.height / 2) - num22);
						if (num32 < maxHomeDistance)
						{
							maxHomeDistance = num32;

							Main.npc[npcFinder].AddBuff(mod.BuffType("Pestilence"), 60);
						}
					}
				}
			}
		}
	}
}
