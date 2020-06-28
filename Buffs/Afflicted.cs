using SpiritMod.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class Afflicted : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Unstable Affliction");
			Description.SetDefault("Falling to pieces");
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().afflicted = true;
			npc.defense -= 5;
			npc.velocity.X *= 0.95f;
			npc.velocity.Y *= 0.95f;

			if(Main.rand.NextBool(4)) {
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.BubbleBlock);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 3f;

				int dust2 = Dust.NewDust(npc.position, npc.width, npc.height, 206);
				Main.dust[dust2].scale = 3f;
			}
		}
	}
}