using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class RailBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Rail Rider");
			Description.SetDefault("Sick shreds, duuude!");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.noFallDmg = true;
		}
	}
}
