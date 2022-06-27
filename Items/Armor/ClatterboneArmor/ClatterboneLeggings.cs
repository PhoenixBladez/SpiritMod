using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ClatterboneArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ClatterboneLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clatterbone Greaves");
		}
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 30;
			Item.value = 5000;
			Item.rare = ItemRarityID.Green;

			Item.defense = 4;
		}
	}
}
