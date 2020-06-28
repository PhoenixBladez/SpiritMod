using SpiritMod.Items.Accessory;
using SpiritMod.Projectiles.Sword;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	public class PrincessScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Princess' Scythe");
			Tooltip.SetDefault("Launches spinning sickles of death");
		}


		public override void SetDefaults()
		{
			item.damage = 100;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.rare = ItemRarityID.Lime;
			item.UseSound = SoundID.Item71;
			item.autoReuse = true;

			item.value = Item.sellPrice(0, 3, 0, 0);
			item.useTurn = true;
			item.crit = 9;
			item.shoot = ModContent.ProjectileType<PrincessSickle>();
			item.shootSpeed = 8f;
		}


		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CrystalShield>(), 1);
			recipe.AddIngredient(ItemID.DeathSickle, 1);
			recipe.AddIngredient(ItemID.ButterflyDust, 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
