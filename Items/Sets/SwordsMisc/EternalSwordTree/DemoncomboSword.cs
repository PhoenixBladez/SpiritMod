using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Items.Sets.SpiritSet;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.SwordsMisc.EternalSwordTree
{
	public class DemoncomboSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eternal Sword");
			Tooltip.SetDefault("Shoots out apparitions of blades");

		}


		public override void SetDefaults()
		{
			item.damage = 84;
			item.useTime = 16;
			item.useAnimation = 16;
			item.melee = true;
			item.width = 60;
			item.height = 64;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 9;
			item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.shootSpeed = 12;
			item.UseSound = SoundID.Item70;
			item.autoReuse = true;
			item.useTurn = true;
			item.shoot = ModContent.ProjectileType<DemonProj>();
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0) {
				target.AddBuff(BuffID.CursedInferno, 180);
			}
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			{
				if (Main.rand.Next(5) == 0) {
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 173);
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale = 1.5f;
					Main.dust[dust].noGravity = true;
				}
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DemoniceSword>(), 1);
			recipe.AddIngredient(ModContent.ItemType<DemonfireSword>(), 1);
			recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 8);
			recipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}