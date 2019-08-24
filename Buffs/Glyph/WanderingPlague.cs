using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Glyph
{
	public class WanderingPlague : ModBuff
	{
		public static int _type;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Wandering Plague");
			Description.SetDefault("");
			Main.pvpBuff[Type] = true;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			return true;
		}


		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().unholyPlague = true;

			if (Main.rand.NextDouble() < 0.25f)
				Dust.NewDust(npc.position, npc.width, npc.height, Dusts.PlagueDust._type, npc.velocity.X, npc.velocity.Y);

			Items.Glyphs.UnholyGlyph.ReleasePoisonClouds(npc, npc.buffTime[buffIndex]);
		}
	}
}
