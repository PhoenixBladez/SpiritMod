using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
	public class DemoniceSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vorpal Sword");
			Tooltip.SetDefault("Shoots out an icy razor that clings to tiles upon hitting them");

		}


		public override void SetDefaults()
		{
			item.damage = 55;
			item.useTime = 22;
			item.useAnimation = 22;
			item.melee = true;
			item.width = 60;
			item.height = 64;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 8;
			item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Lime;
			item.shootSpeed = 12;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.useTurn = true;
			item.shoot = ModContent.ProjectileType<DemonIceProj>();
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0) {
				target.AddBuff(BuffID.Frostburn, 180);
			}
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			{
				if (Main.rand.Next(5) == 0) {
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 206);
					Main.dust[dust].noGravity = true;
				}
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 12);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 6);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}