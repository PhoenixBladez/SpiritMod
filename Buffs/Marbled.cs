using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class Marbled : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Marbled");
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.boss) {
				npc.velocity.X = 0;
				npc.velocity.Y = 0;

				if (Main.rand.NextBool(3)) {
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Marble);
					Main.dust[dust].scale = 2f;
					Main.dust[dust].noGravity = true;
				}
			}
		}
	}
}
