using SpiritMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.GranitechDrones
{
	public class GranitechDroneBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granitech Drone Box");
			Tooltip.SetDefault("Summons 3 drones to aid you\nCan cycle between 3 modes\nThese drones do not take up minion slots");
		}


		public override void SetDefaults()
		{
			item.damage = 72;
			item.summon = true;
			item.knockBack = 1.5f;
			item.width = 24;
			item.height = 24;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<MyPlayer>().granitechDrones = true;
		}
	}
}
