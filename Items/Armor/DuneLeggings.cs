using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class DuneLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dune Leggings");
		}


		int timer = 0;

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 18;
			item.value = 16000;
			item.rare = 5;
			item.vanity = true;
		}
	}
}