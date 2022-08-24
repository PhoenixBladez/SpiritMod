using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.SlagSet;
using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Returning;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SlingHammerSubclass
{
	public class SlagHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slag Breaker");
			Tooltip.SetDefault("Hold down and release to throw the Hammer like a boomerang\nCan be wound up to deal increased damage");
		}

		public override void SetDefaults()
		{
			Item.useStyle = 100;
			Item.width = 40;
			Item.height = 32;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
			Item.noMelee = true;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 8f;
			Item.knockBack = 5f;
			Item.damage = 40;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Orange;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<SlagHammerProj>();
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<SlagHammerProjReturning>()] == 0;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 16);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}