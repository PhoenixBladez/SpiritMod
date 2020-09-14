using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Armor
{
	class ExplorerFight : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Strike Strength");
			Description.SetDefault("You're getting the hang of it!");
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			player.allDamage += modPlayer.damageStacks * 0.03f;
		}

		public override bool ReApply(Player player, int time, int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (modPlayer.damageStacks < 4) {
				modPlayer.damageStacks++;
			}

			return false;
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			MyPlayer modPlayer = Main.player[Main.myPlayer].GetSpiritPlayer();
			tip += $"\nDamage dealt is increased by {modPlayer.damageStacks * 3}%";
			rare = modPlayer.damageStacks >> 1;
		}
	}
}
