using SpiritMod.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class SurgingAnguish : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Surging Anguish");
			Description.SetDefault("'Your mind and body are in great pain'");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}
		float counter = 3f;
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.lifeRegen -= (int)counter;

			if (Main.rand.NextBool(2)) {
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<NightmareDust>());
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}
	}
}