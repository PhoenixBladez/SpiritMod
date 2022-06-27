using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.InfernonDrops
{
	[AutoloadEquip(EquipType.Shield)]
	public class InfernalShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernal Shield");
			Tooltip.SetDefault("Double tap a direction to dash in flames\nReduces damage taken by 5%");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.rare = ItemRarityID.Pink;
			Item.value = 80000;
			Item.damage = 36;
			Item.defense = 3;
			Item.DamageType = DamageClass.Melee;
			Item.accessory = true;

			Item.knockBack = 5f;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().infernalShield = true;
			player.endurance += 0.05f;
		}
	}
}
