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
			Item.width = 52;
			Item.height = 32;
			Item.rare = -11;
			Item.maxStack = 999;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.consumable = true;
			Item.value = Item.buyPrice(0, 6, 0, 0);
		}

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), Mod.Find<ModItem>("AdvPainting" + Main.rand.Next(1, 24)).Type);
			player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), Mod.Find<ModItem>("AdvPainting" + Main.rand.Next(1, 24)).Type);
		}
	}
}
