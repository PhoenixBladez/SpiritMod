using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ClubSubclass.ClubSandwich
{
	public class ClubMealFour : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Club Meal");
			Tooltip.SetDefault("You shouldn't see this");
		}

		public override void SetDefaults()
		{
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 1;
		}

		public override bool ItemSpace(Player player) => true;

		public override bool OnPickup(Player player)
		{
			SoundEngine.PlaySound(SoundID.Item2);
			player.AddBuff(BuffID.WellFed2, 240);
			return false;
		}
	}
}
