using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.MarbleSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MarbleSet.MarbleArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class MarbleLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Treads");
			Tooltip.SetDefault("6% increased movement speed\n'All that glitters is gold'");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = Item.buyPrice(gold: 1, silver: 51);
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxRunSpeed += 0.06f;
			if (player.velocity.X != 0f) {
				int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 0, DustID.GoldCoin);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 13);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
