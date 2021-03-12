using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class MidasTouch : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Midas Touch");
			Description.SetDefault("Enemies drop more gold when killed");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			player.GetModPlayer<MyPlayer>().midasTouch = true;
		}
	}
}