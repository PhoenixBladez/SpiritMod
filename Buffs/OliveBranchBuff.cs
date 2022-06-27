using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class OliveBranchBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Peaceful Resolution");
			Description.SetDefault("'Make peace with your enemies'");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			player.GetModPlayer<MyPlayer>().oliveBranchBuff = true;
		}
	}
}