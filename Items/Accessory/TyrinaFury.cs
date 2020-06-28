
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class TyrinaFury : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tyrina's Fury");
			Tooltip.SetDefault("Increases melee knockback\nWhen under half health, gain 18% increased melee damage and 25% increased melee speed");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.rare = ItemRarityID.Pink;
			item.value = Item.buyPrice(gold: 15);
			item.defense = 3;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if(player.statLife <= player.statLifeMax2 / 2) {
				player.meleeDamage += 0.18f;
				player.meleeSpeed += .25f;
			}
			player.kbGlove = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PowerGlove);
			recipe.AddIngredient(ItemID.WarriorEmblem);
			recipe.AddIngredient(ItemID.SoulofMight, 15);
			recipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
