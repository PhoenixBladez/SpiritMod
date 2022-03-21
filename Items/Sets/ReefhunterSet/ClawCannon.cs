using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.ReefhunterSet.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class ClawCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Claw Cannon");
			Tooltip.SetDefault("'Clannon...clawnon...clawanon...'");
		}

		public override void SetDefaults()
		{
			item.damage = 18;
			item.width = 38;
			item.height = 26;
			item.useTime = item.useAnimation = 37;
			item.mana = 9;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 4;
			item.value = Item.sellPrice(0, 0, 5, 0);
			item.rare = ItemRarityID.Blue;
			item.crit = 6;
			item.autoReuse = true;
			item.noMelee = true;
			item.magic = true;
			item.shootSpeed = 15f;
			item.UseSound = SoundID.Item20;
			item.shoot = ModContent.ProjectileType<Cannonbubble>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 85);

			for (int i = 0; i < 10; ++i)
				Dust.NewDust(position, 0, 0, DustID.BubbleBurst_Blue, speedX * Main.rand.NextFloat(0.65f, 1f), speedY * Main.rand.NextFloat(0.65f, 1f), 0, default, Main.rand.NextFloat(0.75f, 1.5f));

			player.velocity -= new Vector2(speedX, speedY) * 0.15f;
			return true;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		//public override void AddRecipes()
		//{
		//	ModRecipe modRecipe = new ModRecipe(mod);
		//	modRecipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 9);
		//	modRecipe.AddTile(TileID.Anvils);
		//	modRecipe.SetResult(this, 1);
		//	modRecipe.AddRecipe();
		//}
	}
}
