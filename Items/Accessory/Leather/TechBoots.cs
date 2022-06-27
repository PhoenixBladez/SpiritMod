
using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.CoilSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
	[AutoloadEquip(EquipType.Shoes)]
	public class TechBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Cleats");
			Tooltip.SetDefault("Increases movement speed and acceleration slightly\nIncreases movement speed and acceleration further when below half health");
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 20;
			Item.value = Item.buyPrice(0, 0, 70, 0);
			Item.rare = ItemRarityID.Green;

			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.moveSpeed += 0.09f;
			player.runAcceleration += .05f;
			if (player.statLife <= player.statLifeMax2 / 2) {
				player.moveSpeed += 0.09f;
				player.runAcceleration += .05f;
				if (player.velocity.X != 0f) {
					int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 0, DustID.Electric);
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].noGravity = true;
				}
			}
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<LeatherBoots>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TechDrive>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
