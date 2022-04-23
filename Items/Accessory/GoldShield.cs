
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Shield)]
	public class GoldShield : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Shield");
			Tooltip.SetDefault("Provides immunity to Knockback\nAs health decreases, defense increases");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.rare = ItemRarityID.LightRed;
			item.value = 100000;
			item.defense = 4;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GoldenApple>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Leather.LeatherShield>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override void SafeUpdateAccessory(Player player, bool hideVisual)
		{
			player.noKnockback = true;

			if (!player.HasAccessory<MedusaShield>())
			{
				float mult = player.HasAccessory<GoldenApple>() ? 25 : 20f;

				float defBoost = (player.statLifeMax2 - (float)player.statLife) / player.statLifeMax2 * mult;
				player.statDefense += (int)defBoost;
			}
		}
	}
}
