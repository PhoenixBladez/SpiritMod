using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloatingItems.Driftwood.DriftwoodArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class DriftwoodHelmet : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Driftwood Helmet");

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<DriftwoodChestplate>() && legs.type == ModContent.ItemType<DriftwoodLeggings>();

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Allows the wearer to float on water\nIncreased fishing power";
			player.fishingSkill += 5;

			if (player.wet)
				player.velocity.Y = Microsoft.Xna.Framework.MathHelper.Clamp(player.velocity.Y -= 0.35f, -4, 100000);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DriftwoodTileItem>(), 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
