using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Buffs
{
	public class FrenzyVirus : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Frenzy Virus");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.defense = npc.defDefense / 100 * 92;
			npc.lifeRegen -= 14;

			Dust.NewDust(npc.position, npc.width, npc.height, DustID.ShadowbeamStaff);
		}
	}
}
