using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Sets.HuskstalkSet.ElderbarkArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ElderbarkLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark Leggings");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = 0;
			Item.rare = ItemRarityID.White;
			Item.defense = 2;
		}
		public override void UpdateEquip(Player player)
		{
			if (player.velocity.X != 0f) {
				int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 0, DustID.GrassBlades);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].scale *= .6f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 25);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
