using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class Crystal : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Crystallize");
			Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if(!npc.boss) {
				npc.velocity.X *= 0.4f;
				npc.velocity.Y *= 0.4f;

				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Dirt);
				Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.Crystal>());
			}
		}
	}
}