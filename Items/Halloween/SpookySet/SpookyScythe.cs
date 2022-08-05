using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween.SpookySet
{
	public class SpookyScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spooky Scythe");
			Tooltip.SetDefault("Launches homing pumpkins.");
		}

		int counter = 0;
		public override void SetDefaults()
		{
			Item.damage = 67;
			Item.DamageType = DamageClass.Melee;
			Item.width = 45;
			Item.height = 45;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			//  item.shoot = ModContent.ProjectileType<PestilentSwordProjectile>();
			//  item.shootSpeed = 4f;
			Item.autoReuse = true;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			counter++;
			if (counter % 10 == 1) {
				int newProj = Projectile.NewProjectile(player.GetSource_ItemUse(Item), new Vector2(hitbox.X, hitbox.Y), new Vector2(0, 0), ModContent.ProjectileType<Pumpkin>(), Item.damage, 0, player.whoAmI);
				Main.projectile[newProj].DamageType = DamageClass.Melee;

			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.SpookyWood, 12);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}