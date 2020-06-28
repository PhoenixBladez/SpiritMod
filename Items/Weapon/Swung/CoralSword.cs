using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class CoralSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hightide Blade");
			Tooltip.SetDefault("Weapon damage and swing speed increase greatly if the player is underwater\nHitting enemies occasionally causes the sword to splinter and deal extra damage");
		}


		public override void SetDefaults()
		{
			item.damage = 9;
			item.melee = true;
			item.width = 36;
			item.height = 44;
			item.useTime = 26;
			item.useAnimation = 26;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 3;
			item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.shootSpeed = 14f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}
		public override bool CanUseItem(Player player)
		{
			if(player.wet) {
				item.damage = 12;
				item.useTime = 20;
				item.useAnimation = 20;
			} else {
				item.useTime = 26;
				item.useAnimation = 26;
				item.damage = 9;
			}
			return base.CanUseItem(player);
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if(Main.rand.Next(3) == 0) {
				target.StrikeNPC(item.damage / 4, 0f, 0, crit);
				for(int k = 0; k < 20; k++) {
					Dust.NewDust(target.position, target.width, target.height, 225, 2.5f, -2.5f, 0, Color.White, 0.7f);
					Dust.NewDust(target.position, target.width, target.height, 225, 2.5f, -2.5f, 0, default(Color), .34f);
				}
			}
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(ItemID.Coral, 8);
			modRecipe.AddIngredient(ItemID.Seashell, 2);
			modRecipe.AddIngredient(ItemID.BottledWater, 1);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}