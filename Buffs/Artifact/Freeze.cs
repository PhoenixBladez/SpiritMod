using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Artifact
{
	public class Freeze : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Freeze");
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.boss == false)
			{
				npc.velocity.X = 0f;
				npc.velocity.Y = 0f;

				int d = Dust.NewDust(npc.position, npc.width, npc.height, 187);
				Main.dust[d].noGravity = true;
			}
		}
	}
}