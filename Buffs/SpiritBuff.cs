using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class SpiritBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Aura");
			Description.SetDefault("Increases damage and critical strike chance by 5 % \n Getting hurt occasionally spawns a damaging bolt to chase enemies");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			player.GetDamage(DamageClass.Generic) *= 1.05f;
			player.GetCritChance(DamageClass.Generic) += 5;
			modPlayer.spiritBuff = true;
		}
	}
}
