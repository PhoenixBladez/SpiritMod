using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Artifact
{
	public class SunBurn : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Solar Burn");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>(mod).sunBurn = true;
			npc.defense -= 3;
			if (Main.rand.Next(1) == 0)
			{
				int num2 = Dust.NewDust(npc.position, npc.width, npc.height, 244);
				Main.dust[num2].scale = 1.9f;
				Main.dust[num2].velocity *= 1f;
				Main.dust[num2].noGravity = true;
			}
		}
	}
}
