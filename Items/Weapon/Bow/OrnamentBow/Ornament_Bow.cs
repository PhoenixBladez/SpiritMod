using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow.OrnamentBow
{
	public class Ornament_Bow : ModItem
	{
		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 36;
			item.useTime = 36;
			item.width = 12;
			item.height = 28;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = SoundID.Item5;
			item.damage = 18;
			item.shootSpeed = 15f;
			item.knockBack = 1f;
			item.rare = ItemRarityID.Orange;
			item.noMelee = true;
			item.value = Item.sellPrice(gold: 2, silver: 50);
			item.ranged = true;
			item.autoReuse = true;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bow of Ornaments");
			Tooltip.SetDefault("Turns wooden arrows into ornament arrows\nOrnament Arrows create assorted gem arrows upon shattering");
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
				type = mod.ProjectileType("Ornament_Arrow");
			}
			return true;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-1, 0);

		public override void AddRecipes()
		{
			
			ModRecipe recipe2 = new ModRecipe(mod);
			recipe2.AddIngredient(null, "Amethyst_Bow", 1);
			recipe2.AddRecipeGroup("SpiritMod:TopazBows", 1);
			recipe2.AddRecipeGroup("SpiritMod:EmeraldBows", 1);
			recipe2.AddIngredient(null, "Diamond_Bow", 1);
			recipe2.AddIngredient(null, "MarbleChunk", 5);
			recipe2.AddIngredient(null, "GraniteChunk", 5);
			recipe2.AddTile(TileID.Anvils);
			recipe2.SetResult(this);
			recipe2.AddRecipe();
		}
	}
}
