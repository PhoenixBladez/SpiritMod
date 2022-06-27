using SpiritMod.Projectiles.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class PlagueVial : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plague Vial");
			Tooltip.SetDefault("A noxious mixture of flammable toxins\nExplodes into cursed embers upon hitting foes\n'We could make a class out of this!'");
		}


		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 16;
			Item.height = 16;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item106;
			Item.DamageType = DamageClass.Ranged;
			Item.channel = true;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<PlagueVialFriendly>();
			Item.useAnimation = 24;
			Item.useTime = 24;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.shootSpeed = 10.0f;
			Item.damage = 25;
			Item.knockBack = 4.5f;
			Item.value = Item.sellPrice(0, 0, 1, 0);
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = false;
			Item.consumable = true;
		}
	}
}