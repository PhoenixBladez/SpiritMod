using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.DataStructures;
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
			Item.damage = 36;
			Item.knockBack = 1;
			Item.mana = 8;
			Item.width = 26;
			Item.height = 28;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
			Item.buffType = ModContent.BuffType<RabbitOfCaerbannogBuff>();
			Item.shoot = ModContent.ProjectileType<RabbitOfCaerbannog>();
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item44;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)  {
			player.AddBuff(Item.buffType, 2);
			position = Main.MouseWorld;
			return true;
		}
		
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimsonCloak);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddTile(TileID.Loom);
			recipe.Register();
		}
	}
}
