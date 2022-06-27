using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class Wither : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wither");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.lifeRegen -= 18;
			npc.defense -= 5;

			int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood);
			Main.dust[dust].scale = 2f;
			Main.dust[dust].noGravity = true;

			int dust2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.RedTorch);
			Main.dust[dust2].scale = 2f;
			Main.dust[dust2].noGravity = true;
		}

	}
}
