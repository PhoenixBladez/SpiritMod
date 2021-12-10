using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Zones
{
	public class HealingZoneBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Healing Zone");
			Description.SetDefault("You feel invigorated!");
			Main.pvpBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.lifeRegen += 3; 

			if (Main.rand.NextBool(3))
			{
				int d = Dust.NewDust(player.position, player.width, player.height, DustID.Firework_Red, Main.rand.NextFloat(-0.1f, 0.1f), 
					Main.rand.NextFloat(-0.1f, 0.1f), 150, default, Main.rand.NextFloat(0.5f, 0.7f));
				Main.dust[d].noGravity = true;
				Main.dust[d].fadeIn = 0.7f;
			}
		}
	}
}
