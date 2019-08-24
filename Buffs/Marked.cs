using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SpiritMod.Buffs
{
	public class Marked : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Marked");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			int num1 = Dust.NewDust(npc.position, npc.width, npc.height, 244);
			Main.dust[num1].scale = 2.9f;
			Main.dust[num1].velocity *= 3f;
			Main.dust[num1].noGravity = true;
		}

	}
}
