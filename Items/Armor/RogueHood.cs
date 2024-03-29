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
			item.width = 20;
			item.height = 18;
			item.value = Terraria.Item.buyPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Blue;
			item.defense = 1;
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
					player.rangedDamage += 1f;
					player.meleeDamage += 1f;
					player.magicDamage += 1f;
					player.minionDamage += 1f;
				}
			}
		}
	}
}