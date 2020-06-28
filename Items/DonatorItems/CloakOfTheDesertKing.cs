using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	class CloakOfTheDesertKing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cloak of the Desert King");
			Tooltip.SetDefault("Summons a killer bunny");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 28;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.LightRed;
			item.mana = 8;
			item.damage = 36;
			item.knockBack = 1;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.buffType = ModContent.BuffType<RabbitOfCaerbannogBuff>();
			item.shoot = ModContent.ProjectileType<RabbitOfCaerbannog>();
			item.UseSound = SoundID.Item44;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimsonCloak);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddTile(TileID.Loom);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
