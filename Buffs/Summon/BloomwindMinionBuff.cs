using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
    public class BloomwindMinionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Chomper");
			Description.SetDefault("Mother Nature fights back");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (!modPlayer.bloomwindSet)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}

			player.buffTime[buffIndex] = 18000;
		}
	}
}
