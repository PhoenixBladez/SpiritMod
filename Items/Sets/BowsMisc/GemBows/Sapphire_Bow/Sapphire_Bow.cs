using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.BowsMisc.GemBows.Sapphire_Bow
{
	public class Sapphire_Bow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sapphire Bow");
			Tooltip.SetDefault("Turns wooden arrows into sapphire arrows\nSapphire arrows slightly home toward the cursor");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 27;
			Item.useTime = 27;
			Item.width = 12;
			Item.height = 28;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.useAmmo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item5;
			Item.damage = 12;
			Item.shootSpeed = 8f;
			Item.knockBack = 0.5f;
			Item.rare = ItemRarityID.Blue;
			Item.noMelee = true;
            Item.value = Item.sellPrice(0, 0, 67, 50);
            Item.DamageType = DamageClass.Ranged;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
				type = ModContent.ProjectileType<Sapphire_Arrow>();
		}

		public override Vector2? HoldoutOffset() => new Vector2(-1, 0);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBow, 1);
			recipe.AddIngredient(ItemID.Sapphire, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe1 = CreateRecipe();
			recipe1.AddIngredient(ItemID.LeadBow, 1);
			recipe1.AddIngredient(ItemID.Sapphire, 8);
			recipe1.AddTile(TileID.Anvils);
			recipe1.Register();
		}
	}
}
