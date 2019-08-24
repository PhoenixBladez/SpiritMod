using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class Distorted : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Distorted");
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.boss == false)
			{
				npc.velocity.Y = -3;
				if (Main.rand.Next(2) == 0)
				{
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, 110);
					Main.dust[dust].scale = 2f;
					Main.dust[dust].noGravity = true;
				}
			}
		}
	}
}
