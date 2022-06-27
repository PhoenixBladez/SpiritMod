
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
			Item.width = 30;
			Item.height = 28;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.defense = 4;
			Item.accessory = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<GoldenApple>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Leather.LeatherShield>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
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
