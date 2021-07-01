using SpiritMod.Buffs.Pet;
using SpiritMod.Items.Sets.BloodcourtSet;
using SpiritMod.Projectiles.Pet;
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
			Tooltip.SetDefault("Summons a possessed katana that floats above you\nPoints towards the nearest enemy");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Fish);
			item.shoot = ModContent.ProjectileType<SwordPet>();
			item.buffType = ModContent.BuffType<SwordPetBuff>();
			item.value = Terraria.Item.sellPrice(0, 0, 54, 0);
			item.rare = ItemRarityID.Orange;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
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
			recipe.AddIngredient(ModContent.ItemType<BloodFire>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}