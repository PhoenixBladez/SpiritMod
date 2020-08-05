using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class FrostSpine : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Spine");
			Tooltip.SetDefault("Occasionally shoots out a frost bolt");
		}



		public override void SetDefaults()
		{
			item.damage = 11;
			item.noMelee = true;
			item.ranged = true;
			item.width = 24;
			item.height = 38;
			item.useTime = 31;
			item.useAnimation = 31;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 1;
			item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item5;
			item.autoReuse = false;
            item.shootSpeed = 7.8f;
            item.crit = 6;

		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (Main.rand.Next(5) == 1) {
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 8));
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<Projectiles.FrostSpine>(), damage, knockBack, player.whoAmI, 0f, 0f);
			}
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}