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
	public class VineTrap : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Vine Trap");
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.boss == false)
			{
				npc.velocity.X *= .75f;

				if (Main.rand.Next(2) == 0)
				{
					int d= Dust.NewDust(npc.position, npc.width, npc.height, 39);
					Main.dust[d].scale *= Main.rand.NextFloat(.35f, 1.05f);
				}
				npc.GetGlobalNPC<GNPC>(mod).vineTrap = true;
			}
		}
	}
}