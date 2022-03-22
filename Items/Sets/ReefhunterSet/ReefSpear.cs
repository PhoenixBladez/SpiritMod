using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.ReefhunterSet.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class ReefSpear : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Reef Trident");

		public override void SetDefaults()
		{
			item.damage = 18;
			item.width = 28;
			item.height = 14;
			item.useTime = item.useAnimation = 30;
			item.knockBack = 2f;
			item.shootSpeed = 0f;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.melee = true;
			item.channel = false;
			item.autoReuse = true;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(gold: 2);
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<ReefSpearProjectile>();
			item.useStyle = ItemUseStyleID.HoldingOut;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);
		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.shoot = ModContent.ProjectileType<ReefSpearThrown>();
				item.thrown = true;
				item.ranged = false;
				item.damage = 24;
				item.shootSpeed = 12f;
				item.channel = false;
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = item.useAnimation = 35;

			}
			else
			{
				item.shoot = ModContent.ProjectileType<ReefSpearProjectile>();
				item.thrown = false;
				item.ranged = true;
				item.damage = 18;
				item.shootSpeed = 0f;
				item.channel = true;
				item.useTime = item.useAnimation = 40;
				item.useStyle = ItemUseStyleID.HoldingOut;
			}
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
				position -= new Vector2(20, 0);
			return true;
		}

		//public override void AddRecipes()
		//{
		//	var recipe = new ModRecipe(mod);
		//	recipe.AddIngredient(ModContent.ItemType<GranitechMaterial>(), 12);
		//	recipe.AddTile(TileID.MythrilAnvil);
		//	recipe.SetResult(this, 1);
		//	recipe.AddRecipe();
		//}
	}
}