using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
	public class DemonfireSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flameberge Sword");
			Tooltip.SetDefault("Shoots out a fiery bolt");

		}


		public override void SetDefaults()
		{
			item.damage = 49;
			item.useTime = 19;
			item.useAnimation = 19;
			item.melee = true;
			item.width = 60;
			item.height = 64;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 4;
			item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Lime;
			item.shootSpeed = 6;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			item.shoot = ModContent.ProjectileType<FlambergeProjectile>();
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if(Main.rand.Next(4) == 0) {
				target.AddBuff(BuffID.OnFire, 180);
			}
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			{
				if(Main.rand.Next(5) == 0) {
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6);
				}
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FieryEssence>(), 5);
			recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 10);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}