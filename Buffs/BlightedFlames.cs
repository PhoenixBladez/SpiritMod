using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class BlightedFlames : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Blighted Flames");
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.lifeRegen -= 15;
			npc.defense -= 3;

			if (Main.rand.NextBool(2)) {
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 61);
				Main.dust[dust].scale = 3f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}