using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class OracleBoonBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Heresy");
			Description.SetDefault("The gods accept your challenge\nEnchanted enemies appear more often");
		}
	}
}
