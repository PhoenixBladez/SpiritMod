using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
	public class KingslayerVenom : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Kingslayer Venom");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.defense = npc.defDefense / 100 * 82;
			npc.lifeRegen -= 8;

			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 110);
			Main.dust[dust].velocity *= 0f;
		}
	}
}
