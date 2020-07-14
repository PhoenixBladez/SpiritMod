using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class CoralBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hightide Bow");
			Tooltip.SetDefault("Weapon damage and arrow speed increase while the player is underwater\nArrows shot by this bow break apart and deal extra damage");
		}


		int charger;
		public override void SetDefaults()
		{
			item.damage = 9;
			item.noMelee = true;
			item.ranged = true;
			item.width = 28;
			item.height = 44;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = .5f;
			item.value = Item.buyPrice(0, 0, 18, 10);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item5;
			item.autoReuse = false;
			item.shootSpeed = 7.8f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			{
				int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
				Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromCoralBow = true;
			}
			return false;
		}
		public override bool CanUseItem(Player player)
		{
			if(player.wet) {
				item.damage = 11;
				item.useTime = 21;
				item.useAnimation = 21;
				item.shootSpeed = 11.8f;
			} else {
				item.useTime = 25;
				item.useAnimation = 25;
				item.damage = 9;
				item.shootSpeed = 7.8f;
			}
			return base.CanUseItem(player);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, 0);
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(ItemID.Coral, 7);
			modRecipe.AddIngredient(ItemID.Starfish, 3);
			modRecipe.AddIngredient(ItemID.BottledWater, 1);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}