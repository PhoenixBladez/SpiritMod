using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Buffs
{
	public class SoulBurn : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Burn");
			Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().soulBurn = true;

			if (Main.rand.NextBool(2)) {
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Flare_Blue);
				Main.dust[dust].scale = .6f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}