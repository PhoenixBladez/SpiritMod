using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class SatchelReward : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mysterious Satchel");
			Tooltip.SetDefault("'The Painter's been feeling inspired!'\nContains two random paintings");
		}

		public override void SetDefaults()
		{
			item.width = 52;
			item.height = 32;
			item.rare = -11;
			item.maxStack = 999;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.consumable = true;
			item.value = Item.buyPrice(0, 6, 0, 0);
		}

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("AdvPainting" + Main.rand.Next(1, 24)));
			player.QuickSpawnItem(mod.ItemType("AdvPainting" + Main.rand.Next(1, 24)));
		}
	}
}
