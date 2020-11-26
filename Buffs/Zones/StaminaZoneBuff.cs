using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Zones
{
	public class StaminaZoneBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Stamina Zone");
			Description.SetDefault("You feel energized!");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.runAcceleration *= 1.95f;
            player.maxRunSpeed *= 1.64f;
        }
	}
}
