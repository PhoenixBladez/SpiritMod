using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Masks
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class TrapperGlove : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Trapper's Glove");

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.accessory = true;
			Item.value = 1000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
	}
}
