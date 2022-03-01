using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Masks
{
	[AutoloadEquip(EquipType.Head)]
	public class HydraMaskVenom : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Venomous Hydra Mask");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = 5000;
			item.rare = ItemRarityID.Blue;
			item.vanity = true;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class HydraMaskAcid : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acidic Hydra Mask");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = 5000;
			item.rare = ItemRarityID.Blue;
			item.vanity = true;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class HydraMaskFire : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Hydra Mask");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = 5000;
			item.rare = ItemRarityID.Blue;
			item.vanity = true;
		}
	}
}
