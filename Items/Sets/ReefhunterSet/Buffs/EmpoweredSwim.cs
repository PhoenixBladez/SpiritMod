using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Buffs
{
	public class EmpoweredSwim : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Empowered Swim");
			Description.SetDefault("You feel nimbler in water");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.wet)
			{
				player.ignoreWater = true;
				player.accFlipper = true;
			}
		}
	}
}
