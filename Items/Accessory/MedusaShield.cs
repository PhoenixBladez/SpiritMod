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
			Item.width = 28;
			Item.height = 36;
			Item.rare = ItemRarityID.Pink;
			Item.value = 100000;
			Item.accessory = true;
			Item.defense = 6;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<GoldShield>(), 1);
			recipe.AddIngredient(ItemID.PocketMirror, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
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
