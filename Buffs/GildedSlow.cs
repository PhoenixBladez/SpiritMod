using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class GildedSlow : ModBuff
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
			if (!npc.boss)
			{
				npc.velocity.X *= 0.9f;
			}
		}
	}
}
