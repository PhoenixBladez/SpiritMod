using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Buffs
{
	public class WitheringLeaf : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Withering Leaf");
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.defense -= 2;

			if (Main.rand.NextBool(6)) {
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GrassBlades);
				Main.dust[dust].scale = 1f;
			}
		}
	}
}