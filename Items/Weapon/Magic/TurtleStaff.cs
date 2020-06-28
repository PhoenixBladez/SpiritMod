using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class TurtleStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Turtle Staff");
			Tooltip.SetDefault("Surrounds you in slow moving, natural energy");
		}



		public override void SetDefaults()
		{
			item.damage = 68;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.magic = true;
			item.width = 64;
			item.height = 64;
			item.useTime = 52;
			item.mana = 44;
			item.useAnimation = 52;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 8;
			item.value = 90000;
			item.rare = ItemRarityID.Lime;
			item.UseSound = SoundID.Item34;
			item.autoReuse = true;
			item.shootSpeed = 4;
			item.UseSound = SoundID.Item20;
			item.shoot = ModContent.ProjectileType<GrassAura>();
		}

		public override void AddRecipes()
		{

			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<ChloroStaff>(), 1);
			modRecipe.AddIngredient(ItemID.TurtleShell, 3);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
