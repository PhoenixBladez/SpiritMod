using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class CryoCrush : ModBuff
	{
		public static int _type;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Cryo Crush");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>(mod).iceCrush = true;
			if (Main.rand.Next(3) == 0)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 180);
				Main.dust[dust].scale = 1.3f;
				Main.dust[dust].noGravity = true;
			}
		}

	}
}