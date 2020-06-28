using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
	public class RunicPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Runic Pickaxe");
		}


		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 38;
			item.value = 1000;
			item.rare = ItemRarityID.Pink;

			item.pick = 180;

			item.damage = 24;
			item.knockBack = 3;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 9;
			item.useAnimation = 22;

			item.melee = true;
			item.useTurn = true;
			item.autoReuse = true;

			item.UseSound = SoundID.Item1;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 206);
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Rune>(), 15);
			recipe.AddIngredient(ModContent.ItemType<SoulShred>(), 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
