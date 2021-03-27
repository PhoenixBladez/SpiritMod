using Microsoft.Xna.Framework;
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
			item.damage = 36;
			item.knockBack = 1;
			item.mana = 8;
			item.width = 26;
			item.height = 28;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.buffType = ModContent.BuffType<RabbitOfCaerbannogBuff>();
			item.shoot = ModContent.ProjectileType<RabbitOfCaerbannog>();
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item44;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
			player.AddBuff(item.buffType, 2);
			position = Main.MouseWorld;
			return true;
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
