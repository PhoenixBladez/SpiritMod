using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class RoguePants : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rogue Greaves");
			Tooltip.SetDefault("Increases movement speed by 5%");

		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Terraria.Item.buyPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.05f;
		}
	}
}