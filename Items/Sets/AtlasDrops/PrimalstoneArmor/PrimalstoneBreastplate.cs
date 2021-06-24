using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.AtlasDrops.PrimalstoneArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class PrimalstoneBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Primalstone Breastplate");
			Tooltip.SetDefault("Increases life regeneration\n5% increased melee damage\n17% increased melee critical strike chance\n7% increased magic critical strike chance");
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 30;
			item.value = Item.buyPrice(gold: 1);
			item.rare = ItemRarityID.Cyan;
			item.defense = 19;
		}

		public override void UpdateEquip(Player player)
		{
			player.lifeRegen += 2;
			player.meleeCrit += 17;
			player.meleeDamage += .05f;
			player.magicCrit += 7;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ArcaneGeyser>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}