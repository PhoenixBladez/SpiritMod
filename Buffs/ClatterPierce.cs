using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class ClatterPierce : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Clatter Break");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.defense -= 3;
			for (int k = 0; k < 2; k++)
			{
				int d = Dust.NewDust(npc.position, npc.width, npc.height, 147);
				Main.dust[d].scale *= .52f;
			}
			npc.GetGlobalNPC<GNPC>(mod).clatterPierce = true;
		}

	}
}
