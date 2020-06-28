using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class Frostbite : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Bite");
			Tooltip.SetDefault("Occasionally inflicts Frostburn");
		}


		public override void SetDefaults()
		{
			item.damage = 11;
			item.melee = true;
			item.width = 34;
			item.height = 40;
			item.autoReuse = false;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 4;
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 9);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if(Main.rand.Next(7) == 0) {
				target.AddBuff(BuffID.Frostburn, 240, true);
			}
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if(Main.rand.Next(5) == 0) {
				int d = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 68);
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity *= 0f;
			}
		}
	}
}