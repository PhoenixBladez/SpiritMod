using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class HighGravityBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("High Gravity");
			Description.SetDefault("Gravity returns to normal");
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) => player.gravity = 0.4f;
	}
}
