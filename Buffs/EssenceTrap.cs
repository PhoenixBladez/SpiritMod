using SpiritMod.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class EssenceTrap : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Essence Trap");
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().Etrap = true;

			if(Main.rand.NextBool(3)) {
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Vortex);
				Main.dust[dust].noGravity = true;
			}
		}
	}
}