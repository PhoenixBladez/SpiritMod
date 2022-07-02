using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class GildedSlow : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slow");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Terraria.ID.BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.boss) {
				npc.velocity.X *= 0.9f;
			}
		}
	}
}
