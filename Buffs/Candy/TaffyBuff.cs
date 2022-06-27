using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Candy
{
	public class TaffyBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Taffy");
			Description.SetDefault("+4 Defense");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 4;
		}
	}
}
