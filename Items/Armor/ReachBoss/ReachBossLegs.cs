using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ReachBoss
{
	[AutoloadEquip(EquipType.Legs)]
	public class ReachBossLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thornspeaker's Greaves");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.buyPrice(silver: 20);
			Item.rare = ItemRarityID.Green;
			Item.vanity = true;
		}
	}
}
