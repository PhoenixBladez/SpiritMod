using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class Shadowflame : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Shadowflame");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.lifeRegen > 0) {
				player.lifeRegen = 0;
			}

			player.lifeRegen -= 12;

			if (Main.rand.NextBool(2)) {
				Dust.NewDust(player.position, player.width, player.height, DustID.Shadowflame);
			}
		}
	}
}
