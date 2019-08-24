using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class SpectreFury : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("'Wisp Wrath'");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>(mod).spectre = true;
			if (Main.rand.Next(2) == 0)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 66);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}
		}

	}
}