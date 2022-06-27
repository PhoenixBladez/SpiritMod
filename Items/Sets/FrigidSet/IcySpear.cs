using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Thrown.Charge;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FrigidSet
{
	public class IcySpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Javelin");
			Tooltip.SetDefault("Hold and release to throw\nHold it longer for more velocity and damage\nOccaisionally frostburns foes");
		}


		public override void SetDefaults()
		{
			Item.damage = 9;
			Item.noMelee = true;
			Item.channel = true; //Channel so that you can held the weapon [Important]
			Item.rare = ItemRarityID.Blue;
			Item.width = 18;
			Item.height = 18;
			Item.useTime = 15;
			Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 24;
			Item.knockBack = 2.5f;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			//   item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<FrigidJavelinProj>();
			Item.shootSpeed = 0f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 9);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
