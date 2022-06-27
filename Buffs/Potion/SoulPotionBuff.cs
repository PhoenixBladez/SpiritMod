using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Potion
{
	public class SoulPotionBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Guard");
			Description.SetDefault("Reduces damage taken by 5%\nGetting hurt may cause all enemies to suffer 'Soul Burn' for a short time");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			player.endurance += 0.05f;
			modPlayer.soulPotion = true;
		}
	}
}
