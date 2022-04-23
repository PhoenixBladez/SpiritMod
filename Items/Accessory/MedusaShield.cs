using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Shield)]
	public class MedusaShield : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Medusa Shield");
			Tooltip.SetDefault("Provides immunity to knockback and the stoned debuff\nAs health decreases, defense increases");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 36;
			item.rare = ItemRarityID.Pink;
			item.value = 100000;
			item.accessory = true;
			item.defense = 6;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GoldShield>(), 1);
			recipe.AddIngredient(ItemID.PocketMirror, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override void SafeUpdateAccessory(Player player, bool hideVisual)
		{
			player.noKnockback = true;
			player.buffImmune[BuffID.Stoned] = true;

			float mult = 25f;
			if (player.HasAccessory<GoldShield>())
				mult += 6;
			if (player.HasAccessory<GoldenApple>())
				mult += 4;

			float defBoost = (float)(player.statLifeMax2 - player.statLife) / player.statLifeMax2 * mult;
			player.statDefense += (int)defBoost;
		}
	}
}
