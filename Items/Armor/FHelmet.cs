using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class FHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Helmet");
			Tooltip.SetDefault("Increases movement speed by 6%");
		}


		int timer = 0;
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 22;
			item.value = Item.sellPrice(0, 0, 12, 0);
			item.rare = ItemRarityID.Green;
			item.defense = 4;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += .06f;
			player.maxRunSpeed += .03f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<FPlate>() && legs.type == ModContent.ItemType<FLegs>();
		}
		public override void UpdateArmorSet(Player player)
		{
			timer++;

			if(timer == 20) {
				int d = Dust.NewDust(player.position, player.width, player.height, 39);
				Main.dust[d].velocity *= 0f;
				timer = 0;
			}

			player.setBonus = "Killing enemies may drop raw meat, restoring health and granting 'Well Fed'";
			player.GetSpiritPlayer().floranSet = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FloranBar>(), 12);
            recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 7);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
