using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class Marbled : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Marbled");
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.boss == false)
			{
				npc.velocity.X = 0;
				npc.velocity.Y = 0;
				if (Main.rand.Next(3) == 0)
				{
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, 236);
					Main.dust[dust].scale = 2f;
					Main.dust[dust].noGravity = true;
				}
			}
		}
	}
}
