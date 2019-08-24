using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class Wither : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Wither");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.lifeRegen -= 18;
			npc.defense -= 5;
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 5);
				int dust1 = Dust.NewDust(npc.position, npc.width, npc.height, 60);
				Main.dust[dust].scale = 2f;
				Main.dust[dust1].scale = 2f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust1].noGravity = true;
			}
		}

	}
}
