using SpiritMod.Buffs.Armor;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class RogueHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rogue Hood");
			Tooltip.SetDefault("4% increased movement speed");
		}


		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 18;
			Item.value = Terraria.Item.buyPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.04f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<RoguePlate>() && legs.type == ModContent.ItemType<RoguePants>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Getting hit grants four seconds of invisibility and 100% increased damage\n25 second cooldown";
			player.GetSpiritPlayer().rogueSet = true;

			if (player.HasBuff(ModContent.BuffType<RogueCooldown>())) {
				if (player.HasBuff(BuffID.Invisibility)) {
					player.GetDamage(DamageClass.Generic) += 1f;
				}
			}
		}
	}
}