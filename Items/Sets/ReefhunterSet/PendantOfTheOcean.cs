using Terraria.ID;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class PendantOfTheOcean : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pendant of the Ocean");
			Tooltip.SetDefault("Double tap a direction to dash in flames\nReduces damage taken by 5%");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 48;
			item.rare = ItemRarityID.Pink;
			item.value = 80000;
			item.damage = 36;
			item.defense = 3;
			item.melee = true;
			item.accessory = true;
			item.knockBack = 5f;
		}
	}
}
