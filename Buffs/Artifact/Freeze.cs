using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
    public class Freeze : ModBuff
	{
		public override void SetDefaults()
		{
            DisplayName.SetDefault("Freeze");
            Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.boss)
			{
				npc.velocity.X = 0f;
				npc.velocity.Y = 0f;

				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 187);
				Main.dust[dust].noGravity = true;
			}
		}
	}
}