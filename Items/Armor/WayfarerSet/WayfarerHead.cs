using SpiritMod.Items.Material;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WayfarerSet
{
	[AutoloadEquip(EquipType.Head)]
	public class WayfarerHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wayfarer's Hat");
			Tooltip.SetDefault("Immunity to darkness");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Terraria.Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
		}

		public override void UpdateEquip(Player player)
		{
            player.buffImmune[BuffID.Darkness] = true;
        }

		public override void UpdateArmorSet(Player player)
		{
			player.GetSpiritPlayer().wayfarerSet = true;
			player.setBonus = "Killing enemies grants a stacking damage buff\nBreaking pots grants a stacking movement speed buff\nMining ore grants a stacking mining speed buff\nAll buffs stack up to 4 times";
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
			=> body.type == ModContent.ItemType<WayfarerBody>()
			&& legs.type == ModContent.ItemType<WayfarerLegs>();

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Items.Consumable.Quest.DurasilkSheaf>(), 1);
			recipe.AddIngredient(ItemID.IronBar, 1);
			recipe.anyIronBar = true;
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
