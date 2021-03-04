using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.OldCross
{
	public class OldCross : ModItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StaffoftheFrostHydra);
			item.damage = 20;
			item.Size = new Vector2(36, 52);
			item.shoot = mod.ProjectileType("CrossCoffin");
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Green;
			item.useStyle = ItemUseStyleID.HoldingOut;
            ProjectileID.Sets.MinionTargettingFeature[item.shoot] = true;
            item.UseSound = SoundID.Item77;
			item.scale = 0.8f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Old Cross");
			Tooltip.SetDefault("Summons an ancient coffin full of angry skeletons");
		}	
		public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			player.itemRotation = 0; //force rotation to 0, reason magic mirror holdstyle isnt used is because holdout offset is only called for usestyle 5
            if (player.altFunctionUse != 2)
            {
				int num102 = (int)(Main.mouseX + Main.screenPosition.X) / 16; //icky decompiled vanilla code but slightly cleaned up sry
				int num103 = (int)(Main.mouseY + Main.screenPosition.Y) / 16;
				if (player.gravDir == -1f)
					num103 = (int)(Main.screenPosition.Y + Main.screenHeight - Main.mouseY) / 16;

				//loop to find the lowest non-solid tile from the mouse cursor, including platforms, then raises the tile by one to avoid sentry clipping into tiles
				while(num103 < Main.maxTilesY - 10 && Main.tile[num102, num103] != null && !WorldGen.SolidTile2(num102, num103) && Main.tile[num102 - 1, num103] != null && !WorldGen.SolidTile2(num102 - 1, num103) && Main.tile[num102 + 1, num103] != null && !WorldGen.SolidTile2(num102 + 1, num103))
				{
					num103++;
				}
				num103--;
				Projectile proj = Projectile.NewProjectileDirect(new Vector2(Main.mouseX + Main.screenPosition.X, num103 * 16 - 24), Vector2.Zero, type, damage, knockBack, player.whoAmI, -1);
				proj.spriteDirection = player.direction;
				player.UpdateMaxTurrets();
            }
            return false;
        }

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, -4);
		}

		public override bool CanUseItem(Player player)
		{
			int num102 = (int)(Main.mouseX + Main.screenPosition.X) / 16; //icky decompiled vanilla code but slightly cleaned up sry
			int num103 = (int)(Main.mouseY + Main.screenPosition.Y) / 16;
			return !WorldGen.SolidTile(num102, num103) && !WorldGen.SolidTile3(num102, num103);
		}
	}
}
