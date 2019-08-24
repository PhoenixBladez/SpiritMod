
using Terraria;
using Terraria.ModLoader;
using SpiritMod.NPCs;
using Microsoft.Xna.Framework;

namespace SpiritMod.Buffs.Glyph
{
	public class DevouringVoid : ModBuff
	{
		public static int _type;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Devouring Void");
			Description.SetDefault("");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
			canBeCleared = false;
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			return true;
		}
		

		public override void Update(NPC npc, ref int buffIndex)
		{
			GNPC npcData = npc.GetGlobalNPC<GNPC>();

			if (Main.rand.NextDouble() < 0.06f + npcData.voidStacks * (0.072f / Items.Glyphs.VoidGlyph.DELAY))
				Dust.NewDustDirect(npc.position - new Vector2(4), npc.width + 8, npc.height + 8, Dusts.VoidDust._type).customData = npc;
			
			if (npcData.voidStacks > 0)
				npc.buffTime[buffIndex] = 2;
			else
				npc.DelBuff(buffIndex);
		}

	}
}
