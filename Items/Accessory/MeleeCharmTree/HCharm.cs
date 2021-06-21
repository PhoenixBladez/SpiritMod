using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.MeleeCharmTree
{
	public class HCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellstone Charm");
			Tooltip.SetDefault("5% increased attack damage\nAttacks may burn enemies");
		}


		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item11;
			item.accessory = true;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.value = Item.sellPrice(0, 0, 50, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeDamage += 0.05f;
			player.magicDamage += 0.05f;
			player.rangedDamage += 0.05f;
			player.minionDamage += 0.05f;
			player.thrownDamage += 0.05f;
			player.GetSpiritPlayer().hellCharm = true;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ItemID.HellstoneBar, 10);
			modRecipe.AddIngredient(ItemID.Chain, 3);
			modRecipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 1);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
