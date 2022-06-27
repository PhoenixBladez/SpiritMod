using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Buffs.DoT
{
	public class CryoCrush : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryo Crush");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().iceCrush = true;

			if (Main.rand.NextBool(3))
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.DungeonSpirit);
				Main.dust[dust].scale = 1.3f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}