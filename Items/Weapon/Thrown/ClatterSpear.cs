using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Thrown.Charge;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
	public class ClatterSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clatter Javelin");
			Tooltip.SetDefault("Hold and release to throw\nHold it longer for more velocity and damage");
		}


		public override void SetDefaults()
		{
			Item.damage = 13;
			Item.noMelee = true;
			Item.channel = true; //Channel so that you can held the weapon [Important]
			Item.rare = ItemRarityID.Blue;
			Item.width = 18;
			Item.height = 18;
			Item.useTime = 15;
			Item.useAnimation = 45;
            Item.value = 22000;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 24;
			Item.knockBack = 8;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			//   item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<ClatterJavelinProj>();
			Item.shootSpeed = 0f;
		}
	}
}