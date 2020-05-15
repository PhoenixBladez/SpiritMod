using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Potion
{
    public class MushroomPotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Spore Release");
			Description.SetDefault("Leave behind a trail of damaging mushrooms");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			modPlayer.mushroomPotion = true;
		}
	}
}
