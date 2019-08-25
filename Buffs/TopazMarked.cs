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
	public class TopazMarked : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Illuminated");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			int num1 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldCoin);
			Main.dust[num1].scale = Main.rand.NextFloat(.4f, .8f);
			Main.dust[num1].velocity.Y *= -1f;
            Main.dust[num1].velocity.X *= 0f;
			Main.dust[num1].noGravity = true;
		}

	}
}
