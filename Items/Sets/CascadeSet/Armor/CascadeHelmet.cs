using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CascadeSet.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class CascadeHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cascade Helmet");
			Tooltip.SetDefault("Allows for breathing underwater");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = 4800;
			item.rare = ItemRarityID.Orange;
			item.defense = 4;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<CascadeChestplate>() && legs.type == ModContent.ItemType<CascadeLeggings>();

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Dealing damage to enemies builds up a bubble shield,\nwhich at full size grants heavy damage resistance against the next attack,\ndamaging and knocking back nearby enemies.";
			player.GetModPlayer<CascadeArmorPlayer>().setActive = true;
		}

		public override void UpdateEquip(Player player) => player.gills = true;

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DeepCascadeShard>(), 12);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
