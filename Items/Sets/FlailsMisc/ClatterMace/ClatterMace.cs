using SpiritMod.Projectiles.Flail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.FlailsMisc.ClatterMace
{
	public class ClatterMace : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clattering Mace");
			Tooltip.SetDefault("Has a chance to lower enemy defense on hit");
		}


		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 10;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Green;
			Item.damage = 16;
			Item.knockBack = 5.4f;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = Item.useAnimation = 30;
			Item.scale = 1.1F;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<ClatterMaceProj>();
			Item.shootSpeed = 12.5F;
			Item.UseSound = SoundID.Item1;
		}
	}
}