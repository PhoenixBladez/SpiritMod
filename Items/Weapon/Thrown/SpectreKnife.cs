using SpiritMod.Projectiles.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class SpectreKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Knife");
			Tooltip.SetDefault("Upon hitting enemies or tiles, Spectre bolts are releaaed");
		}


		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 24;
			Item.value = Terraria.Item.buyPrice(0, 30, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.maxStack = 999;
			Item.damage = 65;
			Item.knockBack = 3.5f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<SpectreKnifeProj>();
			Item.shootSpeed = 11f;
			Item.UseSound = SoundID.Item1;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(33);
			recipe.AddIngredient(ItemID.SpectreBar, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}