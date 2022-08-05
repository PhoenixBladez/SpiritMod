using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon
{
	public class StarlightStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starlight Staff");
			Tooltip.SetDefault("Summons a stationary twinkle popper");
		}


		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.QueenSpiderStaff); //only here for values we haven't defined ourselves yet
			Item.damage = 71;  //placeholder damage :3
			Item.mana = 40;   //somehow I think this might be too much...? -thegamemaster1234
			Item.width = 40;
			Item.height = 40;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.knockBack = 2.5f;
			Item.UseSound = SoundID.Item25;
			Item.shoot = ModContent.ProjectileType<TwinklePopperMinion>();
			Item.shootSpeed = 0f;
		}
		public override bool CanUseItem(Player player)
		{
			player.FindSentryRestingSpot(Item.shoot, out int worldX, out int worldY, out _);
			worldX /= 16;
			worldY /= 16;
			worldY--;
			return !WorldGen.SolidTile(worldX, worldY);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
			Projectile.NewProjectile(source, worldX, worldY - pushYUp, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
			player.UpdateMaxTurrets();
			return false;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.FragmentStardust, 12);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}