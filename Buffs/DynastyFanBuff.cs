using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class DynastyFanBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Windglider");
			Description.SetDefault("You're still falling, but slower");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.noFallDmg = true;
		}
	}
}
