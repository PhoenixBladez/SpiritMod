using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class SilkHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Manasilk Hood");
            Tooltip.SetDefault("Increases minion damage by 1");
        }

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 22;
			item.value = 2000;
			item.rare = ItemRarityID.Blue;
			item.defense = 1;
		}
        public override void UpdateEquip(Player player)
        {
            player.GetSpiritPlayer().silkenHead = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<SilkRobe>() && legs.type == ModContent.ItemType<SilkLegs>();
		}
		public override void UpdateArmorSet(Player player)
		{

			player.setBonus = "While above 70% health, your minions are 'Mana Infused'\nMana Infused minions deal 1 additional damage and glow";
			if(player.statLife >= player.statLifeMax2 * .7f) {
				player.GetSpiritPlayer().silkenSet = true;
			}

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 3);
			recipe.AddRecipeGroup("SpiritMod:GoldBars");
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}