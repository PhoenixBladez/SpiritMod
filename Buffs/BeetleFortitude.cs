using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	class BeetleFortitude : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Beetle Fortitude");
			Description.SetDefault("Each strike strenghtens you...");
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			player.endurance += modPlayer.beetleStacks * 0.01f;
		}

		public override bool ReApply(Player player, int time, int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (modPlayer.beetleStacks < 15) {
				modPlayer.beetleStacks++;
			}

			return false;
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			MyPlayer modPlayer = Main.player[Main.myPlayer].GetSpiritPlayer();
			tip += $"\nDamage taken is reduced by {modPlayer.beetleStacks}%";
			rare = modPlayer.beetleStacks >> 1;
		}
	}
}
