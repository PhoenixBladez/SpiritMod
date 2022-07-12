using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Buffs.Zones
{
	public class StaminaZoneBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stamina Zone");
			Description.SetDefault("You feel energized!");
			Main.pvpBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.runAcceleration *= 1.20f;
            player.maxRunSpeed *= 1.20f;

			if (Main.rand.NextBool(3))
			{
				int d = Dust.NewDust(player.position, player.width, player.height, DustID.Firework_Green, Main.rand.NextFloat(-0.1f, 0.1f), 
					Main.rand.NextFloat(-0.1f, 0.1f), 150, default, Main.rand.NextFloat(0.5f, 0.7f));
				Main.dust[d].noGravity = true;
				Main.dust[d].fadeIn = 0.7f;
			}
		}
	}
}
