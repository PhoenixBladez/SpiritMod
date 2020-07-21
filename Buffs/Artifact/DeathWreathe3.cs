using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
	public class DeathWreathe3 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Soul Wreathe");
			Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.boss && !npc.friendly) {
				npc.lifeRegen -= 10;

				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 110);
				Main.dust[dust].scale *= 2f;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}