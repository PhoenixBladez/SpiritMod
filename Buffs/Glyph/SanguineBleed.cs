using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;
using Microsoft.Xna.Framework;

namespace SpiritMod.Buffs.Glyph
{
	public class SanguineBleed : ModBuff
	{
		public static int _type;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Crimson Bleed");
			Description.SetDefault("You are rapidly losing blood.");
			Main.buffNoSave[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.debuff[Type] = true;
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			if (time < 357)
				return false;

			npc.buffTime[buffIndex] = 357;
			return true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			GNPC modNPC = npc.GetGlobalNPC<GNPC>();

			double chance = npc.width * npc.height;
			chance = Math.Max(0.02, chance * 0.00003);
			if (!modNPC.sanguinePrev)
			{
				for (int i = 0; i < 4; i++)
				{
					Vector2 offset = Main.rand.NextVec2CircularEven(npc.width >> 1, npc.height >> 1);
					Dust.NewDustPerfect(npc.Center + offset, Dusts.Blood._type).customData = npc;
				}
			}
			if (Main.rand.NextDouble() < chance && npc.buffTime[buffIndex] > 60)
			{
				Vector2 offset = Main.rand.NextVec2CircularEven(npc.width >> 1, npc.height >> 1);
				Dust.NewDustPerfect(npc.Center + offset, Dusts.Blood._type).customData = npc;
			}
			modNPC.sanguineBleed = true;
		}
	}
}
