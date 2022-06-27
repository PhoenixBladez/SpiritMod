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
			Item.CloneDefaults(ItemID.Fish);
			Item.shoot = ModContent.ProjectileType<JellyfishPet>();
			Item.buffType = ModContent.BuffType<JellyfishBuff>();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
				player.AddBuff(Item.buffType, 3600, true);
		}

		public override bool CanUseItem(Player player) => player.miscEquips[0].IsAir;

		public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, 100);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.PeaceCandle, 3);
			recipe.AddIngredient(ItemID.PinkGel, 3);
			recipe.AddIngredient(ItemID.SoulofLight, 3);
			recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
	}
}