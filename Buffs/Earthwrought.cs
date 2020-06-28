using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class Earthwrought : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Earthwrought");
			Description.SetDefault("The Earth seeps through you, increasing life regen");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 3;

			if(Main.rand.NextBool(4)) {
				Dust.NewDust(player.position, player.width, player.height, 44);
			}
		}
	}
}
