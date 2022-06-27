using Microsoft.Xna.Framework;
using SpiritMod.Dusts;
using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Glyph
{
	public class DevouringVoid : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Devouring Void");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
			canBeCleared = false;
		}

		public override bool ReApply(NPC npc, int time, int buffIndex) => true;

		public override void Update(NPC npc, ref int buffIndex)
		{
			GNPC npcData = npc.GetGlobalNPC<GNPC>();

			if (Main.rand.NextDouble() < 0.06f + npcData.voidStacks * (0.072f / Items.Glyphs.VoidGlyph.DELAY)) {
				Dust.NewDustDirect(npc.position - new Vector2(4), npc.width + 8, npc.height + 8, ModContent.DustType<VoidDust>()).customData = npc;
			}

			if (npcData.voidStacks > 0) {
				npc.buffTime[buffIndex] = 2;
			}
			else {
				npc.DelBuff(buffIndex);
			}
		}

	}
}
