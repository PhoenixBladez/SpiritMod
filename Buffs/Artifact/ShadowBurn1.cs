using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Artifact
{
	public class ShadowBurn1 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Shadow Burn");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.lifeRegen = 0;
			npc.lifeRegen -= 8;
			if (Main.rand.Next(6) == 0)
			{
				int num2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Shadowflame);
				Main.dust[num2].scale = 1.9f;
				Main.dust[num2].velocity *= 1f;
				Main.dust[num2].noGravity = true;
				int num3 = Dust.NewDust(npc.position, npc.width, npc.height, 173);
				Main.dust[num3].scale = 1.9f;
				Main.dust[num3].velocity *= 1f;
				Main.dust[num3].noGravity = true;
			}
		}
	}
}
