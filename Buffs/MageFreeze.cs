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
	public class MageFreeze : ModBuff
	{
		public static int _type;

		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Freeze");
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.knockBackResist > 0f)
			{
				npc.velocity.X *= .9f;

				if (Main.rand.Next(5) == 0)
				{
					int d = Dust.NewDust(npc.position, npc.width, npc.height, 172);
					Main.dust[d].noGravity = true;
					Main.dust[d].velocity *= 0f;
					Main.dust[d].scale *= 1.9f;
				}
			}
		}
	}
}