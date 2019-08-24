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
	public class DeathWreathe : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Soul Wreathe");
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.boss == false)
			{
				npc.lifeRegen -= 5;
				int d = Dust.NewDust(npc.position, npc.width, npc.height, 110);
				Main.dust[d].scale *= .6f;
				Main.dust[d].velocity *= 0f;
				Main.dust[d].noGravity = true;
			}
		}
	}
}