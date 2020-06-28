using SpiritMod.Projectiles.Returning;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Returning
{
	public class FleshStick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flesh Stick");
			Tooltip.SetDefault("'It's coming back... ew, ew, ew!'");
		}


		public override void SetDefaults()
		{
			item.damage = 10;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 24;
			item.useAnimation = 24;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5;
			item.value = Terraria.Item.sellPrice(0, 0, 20, 0);
			item.rare = 1;
			item.shootSpeed = 6f;
			item.shoot = ModContent.ProjectileType<FleshStickProj>();
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
		}
		public override bool CanUseItem(Player player)       //this make that you can shoot only 1 boomerang at once
		{
			for(int i = 0; i < 1000; ++i) {
				if(Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot) {
					return false;
				}
			}
			return true;
		}
	}
}