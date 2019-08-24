using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class WitheringLeaf : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Withering Leaf");
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.defense -= 2;

			if (Main.rand.Next(6) == 0)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 3);
				Main.dust[dust].scale = 1f;
			}
		}
	}
}