using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class FrigidWind : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Wind");
			Tooltip.SetDefault("Increases jump height\nYou leave a trail of chilly embers as you walk");
		}


		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 30;
			item.value = 100000;
			item.rare = ItemRarityID.Pink;
			item.defense = 2;
			item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.jumpSpeedBoost += 6f;
			player.GetModPlayer<MyPlayer>().icytrail = true;
		}
	}
}
