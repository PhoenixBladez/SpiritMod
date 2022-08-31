using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MarbleSet.MarbleArmor
{
	[AutoloadEquip(EquipType.Body, EquipType.Legs)]
	public class MarbleChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Robe");
			Tooltip.SetDefault("7% increased movement speed\n'All that glitters is gold'");

			ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
			ArmorIDs.Body.Sets.NeedsToDrawArm[Item.bodySlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = 12100;
			Item.rare = ItemRarityID.Green;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxRunSpeed += 0.08f;
			player.runAcceleration *= 2f;

			if (player.velocity.X != 0f)
			{
				int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 0, DustID.GoldCoin);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
