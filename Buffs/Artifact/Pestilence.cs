using SpiritMod.NPCs;
using System;
using Terraria;
using Terraria.ModLoader;

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
			npc.GetGlobalNPC<GNPC>().pestilence = true;

			if (Main.rand.NextBool(6))
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.Pestilence>());
				Main.dust[dust].scale = 1.9f;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].noGravity = true;

				float maxHomeDistance = 100f;
				for (int npcFinder = 0; npcFinder < Main.maxNPCs; ++npcFinder)
				{
					if (!Main.npc[npcFinder].boss && !Main.npc[npcFinder].townNPC)
					{
                        float homeDistance = Math.Abs(npc.Center.X - Main.npc[npcFinder].Center.X) + Math.Abs(npc.Center.Y - Main.npc[npcFinder].Center.Y);
						if (homeDistance < maxHomeDistance)
						{
							maxHomeDistance = homeDistance;
							Main.npc[npcFinder].AddBuff(ModContent.BuffType<Pestilence>(), 60);
						}
					}
				}
			}
		}
	}
}
