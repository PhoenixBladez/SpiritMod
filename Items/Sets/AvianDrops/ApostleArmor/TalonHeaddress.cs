using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.AvianDrops.ApostleArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class TalonHeaddress : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Apostle's Headdress");
			Tooltip.SetDefault("7% increased magic and ranged critical strike chance");
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 26;
			Item.value = 10000;
			Item.rare = ItemRarityID.Orange;
			Item.defense = 3;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return legs.type == ModContent.ItemType<TalonGarb>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Wind Spirits guide you, granting you double jumps\nMagic and ranged attacks occasionally spawn feathers to attack foes.";
			player.hasJumpOption_Cloud = true;
			player.GetSpiritPlayer().talonSet = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.magicCrit += 7;
			player.rangedCrit += 7;
		}
	}
}
