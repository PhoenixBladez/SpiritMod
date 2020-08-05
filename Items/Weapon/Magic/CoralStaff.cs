using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class CoralStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardrop Staff");
			Tooltip.SetDefault("Weapon damage increases and mana cost decreases while the player is underwater\nShoots a splitting bolt of water");
		}

		public override void SetDefaults()
		{
			item.damage = 8;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.magic = true;
			item.width = 44;
			item.height = 44;
			item.useTime = 23;
			item.mana = 11;
			item.useAnimation = 23;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 3f;
			item.value = Terraria.Item.sellPrice(0, 0, 20, 30);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item72;
			item.autoReuse = false;
			item.shootSpeed = 14;
			item.shoot = ModContent.ProjectileType<StardropStaffProj>();
		}
		public override bool CanUseItem(Player player)
		{
			if (player.wet) {
				item.damage = 10;
				item.mana = 6;
			}
			else {
				item.damage = 8;
				item.mana = 8;
			}
			return base.CanUseItem(player);
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(ItemID.Coral, 10);
			modRecipe.AddIngredient(ItemID.Starfish, 1);
			modRecipe.AddIngredient(ItemID.BottledWater, 1);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
