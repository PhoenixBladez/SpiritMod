using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Buffs
{
	public class VineTrap : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Vine Trap");
			Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.boss) {
				npc.GetGlobalNPC<GNPC>().vineTrap = true;
				npc.velocity.X *= .75f;

				if (Main.rand.NextBool(2)) {
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.JungleGrass);
					Main.dust[dust].scale *= Main.rand.NextFloat(.35f, 1.05f);
				}
			}
		}
	}
}