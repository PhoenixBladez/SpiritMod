using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CryoliteSet.CryoliteArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class CryoliteBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Chestplate");
			Tooltip.SetDefault("12% increased melee damage\nGrants immunity to knockback");
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 26;
			Item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.meleeDamage += 0.12f;
            player.noKnockback = true;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
