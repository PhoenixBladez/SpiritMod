using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Zones
{
	public class LowGravZoneBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Low Gravity Zone");
			Description.SetDefault("You feel light!");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.gravity = .05f;
            if (player.velocity.Y != 0 && player.wings <= 0 && !player.mount.Active)
            {
                player.runAcceleration *= 1.45f;
                player.maxRunSpeed *= 1.2f;
            }
        }
	}
}
