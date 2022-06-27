using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.DonatorItems
{
	public class QuacklingStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quackling Staff");
			Tooltip.SetDefault("Summons a friendly duck to launch aqua bolts at enemies");
		}

		public override void SetDefaults()
		{
			Item.mana = 9;
			Item.damage = 19;
			Item.knockBack = 1;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.width = 26;
			Item.height = 28;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<QuacklingMinion>();
			Item.buffType = ModContent.BuffType<QuacklingBuff>();
			Item.UseSound = SoundID.Item44;
		}
		
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)  {
			player.AddBuff(Item.buffType, 2);
			position = Main.MouseWorld;
			return true;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.Duck, 1);
			recipe.AddIngredient(ItemID.Feather, 10);
			recipe.AddIngredient(ItemID.WaterBolt, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe1 = CreateRecipe(1);
			recipe1.AddIngredient(ItemID.MallardDuck, 1);
			recipe1.AddIngredient(ItemID.Feather, 10);
			recipe1.AddIngredient(ItemID.WaterBolt, 1);
			recipe1.AddTile(TileID.Anvils);
			recipe1.Register();
		}
	}
}