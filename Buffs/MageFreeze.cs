using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Buffs
{
	public class MageFreeze : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chilly Grasp");
			Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.knockBackResist > 0f) {
				npc.velocity.X *= .90f;
				Player player = Main.LocalPlayer;
				if (player.GetSpiritPlayer().cryoSet) {
					npc.velocity.X *= .88f;
				}

				if (Main.rand.NextBool(5)) {
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.DungeonWater);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale *= 1.1f;
				}
			}
		}
	}
}