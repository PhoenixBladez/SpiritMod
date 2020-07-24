using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Flail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Flail
{
	public class MagnetFlail : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Livewire");
			Tooltip.SetDefault("Plugs into tiles, changing the chain into a shocking livewire");

		}


		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 44;
			item.rare = ItemRarityID.Orange;
			item.noMelee = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 34;
			item.useTime = 34;
			item.knockBack = 6;
			item.value = Item.sellPrice(0, 1, 20, 0);
			item.damage = 36;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<MagnetFlailProj>();
			item.shootSpeed = 18f;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.channel = true;
		}
		public override bool CanUseItem(Player player)
		{
			for (int i = 0; i < 1000; ++i) {
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot) {
					return false;
				}
			}
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 25);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}