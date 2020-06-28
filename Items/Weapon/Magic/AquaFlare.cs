using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Magic;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class AquaFlare : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydroflare");
			Tooltip.SetDefault("Summons a controllable ball of water and fire");
		}



		public override void SetDefaults()
		{
			item.damage = 60;
			item.noMelee = true;
			item.magic = true;
			item.width = 64;
			item.height = 64;
			item.useTime = 27;
			item.mana = 17;
			item.useAnimation = 27;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 7;
			item.value = 90000;
			item.channel = true;
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item34;
			item.autoReuse = false;
			item.shootSpeed = 16;
			item.UseSound = SoundID.Item20;
			item.shoot = ModContent.ProjectileType<AquaFlareProj>();
		}
		public override void AddRecipes()
		{

			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ItemID.Flamelash, 1);
			modRecipe.AddIngredient(ItemID.AquaScepter, 1);
			modRecipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 5);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
