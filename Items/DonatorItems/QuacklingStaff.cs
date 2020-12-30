using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
			item.width = 26;
			item.height = 28;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.mana = 9;
			item.damage = 19;
			item.knockBack = 1;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<QuacklingMinion>();
			item.buffType = ModContent.BuffType<QuacklingBuff>();
			item.buffTime = 3600;
			item.UseSound = SoundID.Item44;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Duck, 1);
			recipe.AddIngredient(ItemID.Feather, 10);
			recipe.AddIngredient(ItemID.WaterBolt, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ItemID.MallardDuck, 1);
			recipe1.AddIngredient(ItemID.Feather, 10);
			recipe1.AddIngredient(ItemID.WaterBolt, 1);
			recipe1.AddTile(TileID.Anvils);
			recipe1.SetResult(this, 1);
			recipe1.AddRecipe();
		}
	}
}