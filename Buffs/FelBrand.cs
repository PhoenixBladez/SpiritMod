using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Buffs
{
	public class FelBrand : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Fel Brand");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().felBrand = true;

			if (Main.rand.NextBool(3)) {
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.CursedTorch);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}
		}

	}
}