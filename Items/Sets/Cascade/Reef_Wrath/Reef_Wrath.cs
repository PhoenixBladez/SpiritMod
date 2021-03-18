using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Cascade.Reef_Wrath
{
	public class Reef_Wrath : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 10;
			item.noMelee = true;
			item.noUseGraphic = false;
			item.magic = true;
			item.width = 36;
			item.height = 40;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 5;
			item.shoot = mod.ProjectileType("Reef_Wrath_Projectile_Alt");
			item.shootSpeed = 0f;
			item.knockBack = 8f;
			item.autoReuse = false;
			item.rare = 2;
			item.UseSound = SoundID.Item109;
			item.value = Item.sellPrice(silver: 70);
			item.useTurn = false;
			item.mana = 9;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reef Wrath");
			Tooltip.SetDefault("Conjures harmful coral spires along the ground");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int i = 0; i < 3; i++)
			{
				int randomiseX = Main.rand.Next(-150,151);
				Projectile.NewProjectile((int)(Main.mouseX + Main.screenPosition.X) + randomiseX, (int)(Main.mouseY + Main.screenPosition.Y), 0f, 1f, mod.ProjectileType("Reef_Wrath_Projectile_Alt"), 0, 0f, player.whoAmI);
			}
			
			return false;
		}
	}
}
