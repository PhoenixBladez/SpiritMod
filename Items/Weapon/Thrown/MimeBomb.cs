using SpiritMod.Projectiles.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class MimeBomb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mime Bomb");
			//Tooltip.SetDefault("A noxious mixture of flammable toxins\nExplodes into cursed embers upon hitting foes\n'We could make a class out of this!'");
		}


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.width = 16;
			item.height = 16;
			item.noUseGraphic = true;
			//	item.UseSound = SoundID.Item106;
			item.ranged = true;
			item.channel = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<MimeBombProj>();
			item.useAnimation = 60;
			item.useTime = 60;
			item.consumable = true;
			item.maxStack = 999;
			item.shootSpeed = 5f;
			item.damage = 34;
			item.knockBack = 4.5f;
			item.value = Item.sellPrice(0, 0, 6, 0);
			item.rare = 2;
			item.autoReuse = false;
			item.consumable = true;
		}
	}
}