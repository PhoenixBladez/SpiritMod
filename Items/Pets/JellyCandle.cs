using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Pet;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Pets
{
	public class JellyCandle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jelly Peace Candle");
			Tooltip.SetDefault("Summons a peaceful jellyfish");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Fish);
			item.shoot = ModContent.ProjectileType<JellyfishPet>();
			item.buffType = ModContent.BuffType<JellyfishBuff>();
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
				player.AddBuff(item.buffType, 3600, true);
		}

		public override bool CanUseItem(Player player) => player.miscEquips[0].IsAir;

		public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, 100);

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PeaceCandle, 3);
			recipe.AddIngredient(ItemID.PinkGel, 3);
			recipe.AddIngredient(ItemID.SoulofLight, 3);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}