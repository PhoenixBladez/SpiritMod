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
	public class Crystal : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Crystallize");
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.boss == false)
			{
				npc.velocity.X *= 0.4f;
				npc.velocity.Y *= 0.4f;

				Dust.NewDust(npc.position, npc.width, npc.height, 0);

				Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("Crystal"));
			}
		}
	}
}