using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
	public class Necrosis : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Necrosis");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().necrosis = true;

			if (Main.rand.NextBool(6)) {
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.Pestilence>());
				Main.dust[dust].scale = 1.9f;
				Main.dust[dust].velocity *= 1f;
				Main.dust[dust].noGravity = true;

				int dust2 = Dust.NewDust(npc.position, npc.width, npc.height, 75);
				Main.dust[dust2].scale = 1.9f;
				Main.dust[dust2].velocity *= 1f;
				Main.dust[dust2].noGravity = true;
			}
		}
	}
}
