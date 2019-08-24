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
	public class Slow : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Slow");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.boss == false)
			{
				npc.velocity.X *= 0f;
				npc.velocity.Y *= 0f;
				int num1 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldCoin);
				Main.dust[num1].scale = 2.9f;
				Main.dust[num1].velocity *= 3f;
				Main.dust[num1].noGravity = true;
			}
		}

	}
}
