using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class Brine : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Brine");
			Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.velocity.X *= .9f;
			npc.defense = npc.defDefense / 100 * 97;

			Dust.NewDust(npc.position, npc.width, npc.height, 93);
		}
	}
}