using SpiritMod.Items.Sets.FloatingItems.Driftwood;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.DriftwoodArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class DriftwoodHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Driftwood Helmet");
			Tooltip.SetDefault("12% increased melee speed");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.defense = 2;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<DriftwoodChestplate>();// && legs.type == ModContent.ItemType<CryoliteLegs>();

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Allows the wearer to float on water";

			if (player.wet)
				player.velocity.Y = Microsoft.Xna.Framework.MathHelper.Clamp(player.velocity.Y -= 0.35f, -4, 1000);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DriftwoodTileItem>(), 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
