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

			if (Main.rand.NextBool(2))
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("BlueMoonBlueDust"));
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen -= 8;

			if (Main.rand.NextBool(6))
			{
				int dust = Dust.NewDust(player.position, player.width, player.height, mod.DustType("BlueMoonBlueDust"));
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				int dust2 = Dust.NewDust(player.position, player.width, player.height, mod.DustType("BlueMoonPinkDust"));
				Main.dust[dust2].scale = 1.5f;
				Main.dust[dust2].noGravity = true;
			}
		}
	}
}