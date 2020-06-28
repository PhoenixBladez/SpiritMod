using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class SpiritBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Spirit Aura");
			Description.SetDefault("Increases damage and critical strike chance by 5 % \n Getting hurt occasionally spawns a damaging bolt to chase enemies");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			player.allDamage *= 1.05f;
			player.magicCrit += 5;
			player.meleeCrit += 5;
			player.rangedCrit += 5;
			player.thrownCrit += 5;
			modPlayer.spiritBuff = true;
		}
	}
}
