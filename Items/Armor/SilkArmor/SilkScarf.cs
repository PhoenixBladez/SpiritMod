using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SilkArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class SilkScarf : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Manasilk Scarf");
			Tooltip.SetDefault("Increases your max number of sentries");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = 7500;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
		}
		public override void UpdateEquip(Player player)
		{
			player.maxTurrets += 1;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<SilkRobe>() && legs.type == ModContent.ItemType<SilkLegs>();
		}
		public override void UpdateArmorSet(Player player)
		{

			player.setBonus = "While above 70% health, your minions are 'Mana Infused'\nMana Infused minions deal 1 additional damage and glow";
			if (player.statLife >= player.statLifeMax2 * .7f) {
				player.GetSpiritPlayer().silkenSet = true;
			}

		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.Silk, 3);
			recipe.AddRecipeGroup("SpiritMod:GoldBars");
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}