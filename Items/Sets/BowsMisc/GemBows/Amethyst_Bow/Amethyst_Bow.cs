using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BowsMisc.GemBows.Amethyst_Bow
{
	public class Amethyst_Bow : ModItem
	{
		public override void SetDefaults()
		{
			item.useStyle = 5;
			item.useAnimation = 29;
			item.useTime = 29;
			item.width = 12;
			item.height = 28;
			item.shoot = 1;
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = SoundID.Item5;
			item.damage = 10;
			item.shootSpeed = 7f;
			item.knockBack = 0.5f;
			item.rare = ItemRarityID.White;
			item.noMelee = true;
            item.value = Terraria.Item.sellPrice(0, 0, 22, 50);
            item.ranged = true;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Bow");
			Tooltip.SetDefault("Turns wooden arrows into amethyst arrows\nAmethyst arrows have an increased critical strike chance");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = mod.ProjectileType("Amethyst_Arrow");
			}
			return true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0);
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CopperBow, 1);
			recipe.AddIngredient(ItemID.Amethyst, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ItemID.TinBow, 1);
			recipe1.AddIngredient(ItemID.Amethyst, 8);
			recipe1.AddTile(TileID.Anvils);
			recipe1.SetResult(this);
			recipe1.AddRecipe();
		}
	}
}
