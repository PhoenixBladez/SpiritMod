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
			Item.width = 22;
			Item.height = 20;
			Item.value = 5000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
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
			Item.width = 22;
			Item.height = 20;
			Item.value = 5000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
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
			Item.width = 22;
			Item.height = 20;
			Item.value = 5000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
	}
}
