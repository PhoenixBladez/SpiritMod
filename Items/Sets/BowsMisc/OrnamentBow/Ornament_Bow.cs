using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BowsMisc.OrnamentBow
{
	public class Ornament_Bow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bow of Ornaments");
			Tooltip.SetDefault("Turns wooden arrows into ornament arrows\nOrnament Arrows create assorted gem arrows upon shattering");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 36;
			Item.useTime = 36;
			Item.width = 12;
			Item.height = 28;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.useAmmo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item5;
			Item.damage = 18;
			Item.shootSpeed = 15f;
			Item.knockBack = 1f;
			Item.rare = ItemRarityID.Orange;
			Item.noMelee = true;
			Item.value = Item.sellPrice(gold: 2, silver: 50);
			Item.DamageType = DamageClass.Ranged;
			Item.autoReuse = true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;

			if (type == ProjectileID.WoodenArrowFriendly)
				type = ModContent.ProjectileType<Ornament_Arrow>();
		}

		public override Vector2? HoldoutOffset() => new Vector2(-1, 0);

		public override void AddRecipes()
		{
			
			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "Amethyst_Bow", 1);
			recipe2.AddRecipeGroup("SpiritMod:TopazBows", 1);
			recipe2.AddRecipeGroup("SpiritMod:EmeraldBows", 1);
			recipe2.AddIngredient(null, "Diamond_Bow", 1);
			recipe2.AddIngredient(null, "MarbleChunk", 5);
			recipe2.AddIngredient(null, "GraniteChunk", 5);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
