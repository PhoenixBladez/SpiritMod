using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.ScarabeusDrops.ChitinArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class ChitinHelmet : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Chitin Faceguard");

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(silver: 14);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 3;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<ChitinChestplate>() && legs.type == ModContent.ItemType<ChitinLeggings>();

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Double tap in a direction to dash and envelop yourself in a tornado";
			player.GetSpiritPlayer().chitinSet = true;

			if (player.velocity.X != 0f) {
				int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 0, DustID.Dirt);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}

		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Chitin>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
