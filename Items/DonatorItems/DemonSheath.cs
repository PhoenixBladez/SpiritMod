using Microsoft.Xna.Framework;
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
			Item.CloneDefaults(ItemID.Fish);
			Item.shoot = ModContent.ProjectileType<SwordPet>();
			Item.buffType = ModContent.BuffType<SwordPetBuff>();
			Item.value = Terraria.Item.sellPrice(0, 0, 54, 0);
			Item.rare = ItemRarityID.Orange;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(Item.buffType, 3600, true);
			}
		}

		public override bool CanUseItem(Player player)
		{
			return player.miscEquips[0].IsAir;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.Muramasa, 1);
			recipe.AddIngredient(ModContent.ItemType<DreamstrideEssence>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}