using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Artifact
{
	public class Necrosis : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Necrosis");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>(mod).necrosis = true;
			if (Main.rand.Next(6) == 0)
			{
				int num1 = Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("Pestilence"));
				Main.dust[num1].scale = 1.9f;
				Main.dust[num1].velocity *= 1f;
				Main.dust[num1].noGravity = true;
				int num2 = Dust.NewDust(npc.position, npc.width, npc.height, 75);
				Main.dust[num2].scale = 1.9f;
				Main.dust[num2].velocity *= 1f;
				Main.dust[num2].noGravity = true;
			}
		}
	}
}
