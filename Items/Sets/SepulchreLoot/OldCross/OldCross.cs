using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.OldCross
{
	public class OldCross : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StaffoftheFrostHydra);
			Item.damage = 16;
			Item.Size = new Vector2(36, 52);
			Item.shoot = ModContent.ProjectileType<CrossCoffin>();
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Green;
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item77;
			Item.scale = 0.8f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Old Cross");
			Tooltip.SetDefault("Summons an ancient coffin full of angry skeletons");
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
        {
			player.itemRotation = 0; //force rotation to 0, reason magic mirror holdstyle isnt used is because holdout offset is only called for usestyle 5
            if (player.altFunctionUse != 2)
            {
				int x = (int)(Main.mouseX + Main.screenPosition.X) / 16; //icky decompiled vanilla code but slightly cleaned up sry
				int y = (int)(Main.mouseY + Main.screenPosition.Y) / 16;
				if (player.gravDir == -1f)
					y = (int)(Main.screenPosition.Y + Main.screenHeight - Main.mouseY) / 16;

				//loop to find the lowest non-solid tile from the mouse cursor, including platforms, then raises the tile by one to avoid sentry clipping into tiles
				while(y < Main.maxTilesY - 10 && Main.tile[x, y] != null && !WorldGen.SolidTile2(x, y) && Main.tile[x - 1, y] != null && !WorldGen.SolidTile2(x - 1, y) && Main.tile[x + 1, y] != null && !WorldGen.SolidTile2(x + 1, y))
					y++;

				y--;
				Projectile proj = Projectile.NewProjectileDirect(new Vector2(Main.mouseX + Main.screenPosition.X, y * 16 - 24), Vector2.Zero, type, damage, knockback, player.whoAmI, -1);
				proj.spriteDirection = player.direction;
				player.UpdateMaxTurrets();
            }
            return false;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-6, -4);

		public override bool CanUseItem(Player player)
		{
			int num102 = (int)(Main.mouseX + Main.screenPosition.X) / 16; //icky decompiled vanilla code but slightly cleaned up sry
			int num103 = (int)(Main.mouseY + Main.screenPosition.Y) / 16;
			return !WorldGen.SolidTile(num102, num103) && !WorldGen.SolidTile3(num102, num103);
		}
	}
}
