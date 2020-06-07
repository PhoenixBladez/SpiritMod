using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
    public class Crystallize : ModBuff
	{
		public override void SetDefaults()
		{
            DisplayName.SetDefault("Crystallize");
            Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.boss)
			{
				npc.velocity.X = 0f;
				npc.velocity.Y = 0f;

				Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.Crystal>());
			}
		}
	}
}