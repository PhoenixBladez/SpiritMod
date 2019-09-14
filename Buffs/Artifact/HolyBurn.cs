using SpiritMod.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
    public class HolyBurn : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Holy Burn");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>(mod).holyBurn = true;

			if (Main.rand.NextBool(2))
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldCoin);
				Main.dust[dust].scale = 1.9f;
				Main.dust[dust].velocity *= 1f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
