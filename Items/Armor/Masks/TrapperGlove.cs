using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Masks
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class TrapperGlove : ModItem
	{
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trapper's Glove");
		}


		int timer = 0;
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 22;
			item.accessory = true;
			item.value = 1000;
			item.rare = ItemRarityID.Blue;
			item.vanity = true;
		}
	}
}
