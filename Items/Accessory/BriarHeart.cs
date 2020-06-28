using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class BriarHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Briar Heart");
			Tooltip.SetDefault("5% increased melee damage and speed\n9% increased melee critical srike chance\n7% increased ranged critical strike chance\n5% increased magic critical strike chance\nMagic attacks may inflict Cursed Inferno and Ichor\nGetting hurt may cause all attacks to inflict poison for a short while\nIncreases maximum mana by 50\nIncreases maximum life by 20");
		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 42;
			item.value = Item.buyPrice(0, 1, 20, 0);
			item.rare = ItemRarityID.LightPurple;
			item.defense = 3;

			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.magicCrit += 5;
			player.meleeCrit += 9;
			player.rangedCrit += 7;
			player.meleeDamage += 0.05f;
			player.statLifeMax2 += 20;
			player.meleeSpeed += 0.05f;
			player.GetSpiritPlayer().briarHeart = true;
			{
				player.statManaMax2 += 50;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HuntingNecklace>(), 1);
			recipe.AddIngredient(ModContent.ItemType<GremlinTooth>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Acid>(), 4);
			recipe.AddRecipeGroup("SpiritMod:EvilNecklace", 1);
			recipe.AddIngredient(ItemID.SoulofNight, 8);
			recipe.AddIngredient(ItemID.SoulofLight, 8);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
	}
}