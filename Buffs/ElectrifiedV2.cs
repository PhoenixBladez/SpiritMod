using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class ElectrifiedV2 : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Electrified");
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.lifeRegen -= 30;
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 226);
			}
		}

	}
}
