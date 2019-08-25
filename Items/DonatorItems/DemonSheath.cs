using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	public class DemonSheath : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demon Sheath");
			Tooltip.SetDefault("Summons a possessed katana that floats above you\nPoints towards the nearest enemy\n~Donator Item~");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Fish);
			item.shoot = mod.ProjectileType("SwordPet");
			item.buffType = mod.BuffType("SwordPetBuff");
			item.value = Terraria.Item.sellPrice(0, 0, 54, 0);
			item.rare = 3;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}

		public override bool CanUseItem(Player player)
		{
			return player.miscEquips[0].IsAir;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Muramasa, 1);
			recipe.AddIngredient(mod.ItemType("BloodFire"), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}