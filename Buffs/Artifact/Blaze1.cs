using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Artifact
{
	public class Blaze1 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Blaze");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>(mod).blaze1 = true;
			int num2 = Dust.NewDust(npc.position, npc.width, npc.height, 244);
			Main.dust[num2].scale = 5.9f;
			Main.dust[num2].velocity *= 1f;
			Main.dust[num2].noGravity = true;
		}
	}
}
