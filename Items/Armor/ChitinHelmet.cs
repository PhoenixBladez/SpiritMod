using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ChitinHelmet : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Chitin Faceguard");

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = 8000;
			item.rare = ItemRarityID.Blue;
			item.defense = 3;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<ChitinChestplate>() && legs.type == ModContent.ItemType<ChitinLeggings>();

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Double tap in a direction to dash and envelop yourself in a tornado, damaging any enemies caught within it";
			player.GetSpiritPlayer().chitinSet = true;

			if (player.velocity.X != 0f) {
				int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 0, 0);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Chitin>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
