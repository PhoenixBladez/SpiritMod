
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GoreArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class IchorMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Mask");
			Tooltip.SetDefault("10% increased melee damage\n6% increased melee critical strike chance");
		}

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.LightRed;
			item.defense = 11;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) 
			=> body.type == ModContent.ItemType<IchorPlate>() && legs.type == ModContent.ItemType<IchorLegs>();

		public override void UpdateArmorSet(Player player)
		{
			string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN");
			player.setBonus = $"Double tap {tapDir} to cause damage all nearby enemies and suffer Ichor for a long period of time\n1 minute cooldown";
			player.GetSpiritPlayer().ichorSet1 = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.meleeDamage += 0.1f;
			player.meleeCrit += 6;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FleshClump>(), 11);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
