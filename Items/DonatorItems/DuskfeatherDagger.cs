using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	class DuskfeatherDagger : ModItem
	{
		public static readonly int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duskfeather Dagger");
			Tooltip.SetDefault(
				"Can throw up to eight Duskfeather blades\n" +
				"Right-click to recall all deployed blades\n" +
				"Can be Equipped to summon a small Harpy pet\n~Donator Item~");
		}

		public override void SetDefaults()
		{
			item.width = 42;
			item.height = 42;
			item.useStyle = 1;

			item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			item.rare = 3;

			item.damage = 24;
			item.crit = 16;
			item.knockBack = 3f;
			item.thrown = true;
			item.autoReuse = true;
			item.shootSpeed = 16f;
			item.UseSound = SoundID.Item1;
			item.noUseGraphic = true;
			item.noMelee = true;

			item.useTime = 18;
			item.useAnimation = 18;

			item.buffType = HarpyPetBuff._type;
			//Don't change this line, or the item will break.
			item.shoot = Projectiles.DonatorItems.HarpyPet._type;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			//Don't put this line into SetDefaults, or the item will break.
			item.shoot = Projectiles.DonatorItems.DuskfeatherBlade._type;
			if (player.altFunctionUse == 2)
			{
				if (item.useStyle == 1)
				{
					item.useStyle = 4;
					item.noUseGraphic = false;
					item.UseSound = null;
				}
				else
					return false;
			}
			else
			{
				if (player.ownedProjectileCounts[Projectiles.DonatorItems.DuskfeatherBlade._type] >= 8)
					Projectiles.DonatorItems.DuskfeatherBlade.AttractOldestBlade(player);
				item.useStyle = 1;
				item.noUseGraphic = true;
				item.UseSound = SoundID.Item1;
			}
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{
				Projectiles.DonatorItems.DuskfeatherBlade.AttractBlades(player);
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
