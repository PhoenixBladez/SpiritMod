using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.AtlasDrops.PrimalstoneArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class PrimalstoneLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Primalstone Leggings");
			Tooltip.SetDefault("9% increased melee and magic damage\n5% reduced mana usage\n10% reduced movement speed");
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 30;
			item.value = 10;
			item.rare = ItemRarityID.Cyan;
			item.defense = 17;
		}

		public override void UpdateEquip(Player player)
		{
			player.meleeDamage += 0.09f;
			player.magicDamage += 0.09f;
			player.manaCost -= .05f;
			player.moveSpeed -= .1f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ArcaneGeyser>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}