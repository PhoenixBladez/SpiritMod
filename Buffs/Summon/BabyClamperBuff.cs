using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
    public class BabyClamperBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Baby Clamper");
			Description.SetDefault("It's young and jumpy!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (!modPlayer.oceanSet)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}

			player.buffTime[buffIndex] = 18000;
		}
	}
}
