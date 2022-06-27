using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Buffs
{
	public class SoulFlare : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Flare");
			Description.SetDefault("Your soul is fluctuating...");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.lifeRegen > 0)
				player.lifeRegen = 0;

			player.lifeRegen -= 17;
			player.statDefense -= 4;

			if (Main.rand.NextBool(4))
				Dust.NewDust(player.position, player.width, player.height, DustID.Flare_Blue);
		}
	}
}
