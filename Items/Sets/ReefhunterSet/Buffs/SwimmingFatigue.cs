using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Buffs
{
	public class SwimmingFatigue : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Swimming Fatigue");
			Description.SetDefault("You can't swim very well for a bit");

			Main.debuff[Type] = true;
		}
	}
}
