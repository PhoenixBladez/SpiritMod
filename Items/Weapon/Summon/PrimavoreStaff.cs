using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Weapon.Summon
{
	public class PrimavoreStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Primavore Staff");
			Tooltip.SetDefault("Summons a stationary Primavore");
		}


		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.QueenSpiderStaff); //only here for values we haven't defined ourselves yet
			item.damage = 52;  //placeholder damage :3
			item.mana = 30;   //somehow I think this might be too much...? -thegamemaster1234
			item.width = 40;
			item.height = 40;
			item.value = Terraria.Item.sellPrice(0, 6, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.knockBack = 2.5f;
			item.UseSound = SoundID.Item25;
			item.shoot = ModContent.ProjectileType<Primavore>();
			item.shootSpeed = 0f;
		}

		public override bool CanUseItem(Player player)
		{
			player.FindSentryRestingSpot(item.shoot, out int worldX, out int worldY, out _);
			worldX /= 16;
			worldY /= 16;
			worldY-= 4;
			return !WorldGen.SolidTile(worldX, worldY);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
			worldY -= 32;
			Projectile.NewProjectile(worldX, worldY - pushYUp, speedX, speedY, type, damage, knockBack, player.whoAmI);
			player.UpdateMaxTurrets();
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PrimevalEssence>(), 14);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.EssenceDistorter>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
