using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Armor
{
	class ExplorerMine : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explorer's Strength");
			Description.SetDefault("Keep digging!");
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			player.pickSpeed -= modPlayer.miningStacks * 0.05f;
        }

		public override bool ReApply(Player player, int time, int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (modPlayer.miningStacks < 4) {
				modPlayer.miningStacks++;
			}

			return false;
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			MyPlayer modPlayer = Main.player[Main.myPlayer].GetSpiritPlayer();
			tip += $"\nMining speed is increased: {modPlayer.miningStacks} stacks";
			rare = modPlayer.miningStacks >> 1;
		}
	}
}
