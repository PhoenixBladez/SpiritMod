using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Armor
{
	class ExplorerPot : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Explorer's Vigor");
			Description.SetDefault("You're eager for more!");
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			player.moveSpeed += modPlayer.movementStacks * 0.05f/4;
            player.runAcceleration += modPlayer.movementStacks * 0.015f/4;
        }

		public override bool ReApply(Player player, int time, int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (modPlayer.movementStacks < 16) {
				modPlayer.movementStacks++;
			}

			return false;
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			MyPlayer modPlayer = Main.player[Main.myPlayer].GetSpiritPlayer();
			tip += $"\nMovement speed is increased by {modPlayer.movementStacks * 5/4}%";
			rare = modPlayer.movementStacks >> 1;
		}
	}
}
