using SpiritMod.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
    public class SunBurn : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Solar Burn");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().sunBurn = true;
			npc.defense -= 3;

			if (Main.rand.NextBool(2))
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.CopperCoin);
				Main.dust[dust].scale = 1.9f;
				Main.dust[dust].velocity *= 1f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
