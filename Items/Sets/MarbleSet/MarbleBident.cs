using SpiritMod.Projectiles.Held;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MarbleSet
{
	public class MarbleBident : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Bident");
			Tooltip.SetDefault("Occasionally inflicts 'Midas', causing enemies to drop more gold\nHitting an enemy that has 'Midas' may release a fountain of gold");
		}


		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.width = 50;
			Item.height = 50;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.useAnimation = 28;
			Item.useTime = 28;
			Item.shootSpeed = 4f;
			Item.knockBack = 8f;
			Item.damage = 18;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<MarbleBidentProj>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
