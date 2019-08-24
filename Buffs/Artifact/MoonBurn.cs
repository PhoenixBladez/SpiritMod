using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Artifact
{
	public class MoonBurn : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Moon Burn");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>(mod).moonBurn = true;
			if (Main.rand.Next(1) == 0)
			{
				int num2 = Dust.NewDust(npc.position, npc.width, npc.height, 173);
				Main.dust[num2].scale = 1.9f;
				Main.dust[num2].velocity *= 1f;
				Main.dust[num2].noGravity = true;
			}
		}
	}
}
