using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.AvianDrops
{
	public class SkeletalonStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeletalon Staff");
			Tooltip.SetDefault("Summons an army of fossilized birds to fight for you");
		}


		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 28;
			item.value = Item.sellPrice(0, 3, 25, 0);
			item.rare = ItemRarityID.Green;
			item.mana = 12;
			item.damage = 13;
			item.knockBack = 3;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<SkeletalonMinion>();
			item.UseSound = SoundID.Item44;
		}
		public override bool AltFunctionUse(Player player) => true;

		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
				player.MinionNPCTargetAim();
			return player.altFunctionUse == 2;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			for (int i = 0; i <= Main.rand.Next(0, 3); i++) {
				int proj = Terraria.Projectile.NewProjectile(position.X + Main.rand.Next(-30, 30), position.Y, 0f, 0f, type, damage, knockBack, player.whoAmI);
				Projectile projectile = Main.projectile[proj];
				for (int j = 0; j < 10; j++) {
					int d = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.Dirt, (float)(Main.rand.Next(5) - 2), (float)(Main.rand.Next(5) - 2), 133);
					Main.dust[d].scale *= .75f;
				}
			}
			return false;
		}
	}
}