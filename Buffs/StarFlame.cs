using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class StarFlame : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Star Flame");
			Description.SetDefault("'An astral force saps your vitality'");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.lifeRegen -= 8;

			if(Main.rand.NextBool(2)) {
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 206);
				Main.dust[dust].scale = .85f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen -= 8;

			if(Main.rand.NextBool(4)) {
				int dust = Dust.NewDust(player.position, player.width, player.height, 187);
				Main.dust[dust].scale = 1.25f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				int dust2 = Dust.NewDust(player.position, player.width, player.height, 206);
				Main.dust[dust].scale = .95f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].noLight = true;
			}
		}
	}
}