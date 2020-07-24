using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
	public class DeathWreathe : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Soul Wreathe");
			Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.boss) {
				npc.lifeRegen -= 5;

				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 110);
				Main.dust[dust].scale *= .6f;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}