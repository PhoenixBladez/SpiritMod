using SpiritMod.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class SpectreFury : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("'Wisp Wrath'");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>(mod).spectre = true;

			if (Main.rand.NextBool(2))
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Rainbow);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}
		}

	}
}