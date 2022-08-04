using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BotanistSet
{
	internal class BotanistPlayer : ModPlayer
	{
		public bool active = false;

		public override void ResetEffects() => active = false;
	}
}
