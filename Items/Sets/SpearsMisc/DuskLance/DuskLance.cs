using SpiritMod.Items.BossLoot.DuskingDrops;
using SpiritMod.Projectiles.Held;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpearsMisc.DuskLance
{
	public class DuskLance : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Lance");
			Tooltip.SetDefault("Occasionally shoots out an apparition that inflicts Shadowflame");
		}
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.width = 24;
			Item.height = 24;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.useAnimation = 28;
			Item.useTime = 28;
			Item.shootSpeed = 5.5f;
			Item.knockBack = 6f;
			Item.damage = 38;
			Item.value = Item.sellPrice(0, 3, 60, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.shoot = ModContent.ProjectileType<DuskLanceProj>();
		}
		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[Item.shoot] > 0)
				return false;
			return true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.DarkLance, 1);
			recipe.AddIngredient(ModContent.ItemType<DuskStone>(), 4);
			recipe.AddIngredient(ItemID.SoulofNight, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
