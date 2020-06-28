using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	class DuskfeatherDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duskfeather Dagger");
			Tooltip.SetDefault(
				"Can throw up to eight Duskfeather blades\n" +
				"Right-click to recall all deployed blades\n" +
				"Can be Equipped to summon a small Harpy pet");
		}

		public override void SetDefaults()
		{
			item.width = 42;
			item.height = 42;
			item.useStyle = ItemUseStyleID.SwingThrow;

			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Orange;

			item.damage = 24;
			item.crit = 16;
			item.knockBack = 3f;
			item.ranged = true;
			item.autoReuse = true;
			item.shootSpeed = 16f;
			item.UseSound = SoundID.Item1;
			item.noUseGraphic = true;
			item.noMelee = true;

			item.useTime = 18;
			item.useAnimation = 18;

			item.buffType = ModContent.BuffType<HarpyPetBuff>();
			//Don't change this line, or the item will break.
			item.shoot = ModContent.ProjectileType<HarpyPet>();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			//Don't put this line into SetDefaults, or the item will break.
			item.shoot = ModContent.ProjectileType<DuskfeatherBlade>();
			if(player.altFunctionUse == 2) {
				if(item.useStyle == ItemUseStyleID.SwingThrow) {
					item.useStyle = ItemUseStyleID.HoldingUp;
					item.noUseGraphic = false;
					item.UseSound = null;
				} else
					return false;
			} else {
				if(player.ownedProjectileCounts[ModContent.ProjectileType<DuskfeatherBlade>()] >= 8)
					DuskfeatherBlade.AttractOldestBlade(player);
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.noUseGraphic = true;
				item.UseSound = SoundID.Item1;
			}
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(player.altFunctionUse == 2) {
				DuskfeatherBlade.AttractBlades(player);
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Muramasa);
			recipe.AddIngredient(ItemID.Feather, 8);
			recipe.AddIngredient(ItemID.FossilOre, 25);
			recipe.AddIngredient(ItemID.Amber, 8);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
